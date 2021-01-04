using ADServiceLibCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServiceLibCore.Services
{
    public class ADService : IADService
    {
        private IADServiceDb _db;

        public ADService(IADServiceDb db)
        {
            _db = db;
            _httpContext = db.HttpContext;
        }

        private HttpContext _httpContext;

        #region Domain Login
        public async Task Logout()
        {
            if (_httpContext != null)
            {
                if (_httpContext.User.Identity.IsAuthenticated)
                {
                    await _httpContext.SignOutAsync();
                }
            }
        }

        public async Task<IADResult<DomainInfo>> Login(string loginName, string password)
        {
            var result = new ADResult<DomainInfo>();
            var query = await GetDomainInfoInDatabase();
            try
            {
                if (!query.Success)
                {
                    return result.ToReturn(query.Error);
                }

                var domain = query.Value;
                domain.AdminName = loginName;
                domain.AdminPassword = password;

                using var context = new PrincipalContext(ContextType.Domain, domain.DomainName, domain.ContainerString, domain.AdminName, domain.AdminPassword);
                domain.ConnectedServer = context.ConnectedServer; // throw a exception if server cannot connect.

                if (string.IsNullOrEmpty(domain.ConnectedServer))
                {
                    return result.ToReturn("Connect to Domain Server fail.");
                }

                var login = await new ADLoginHelper().SignIn(domain, _httpContext);
                if (login.Success)
                {
                    return result.ToReturn(login.Value);
                }
                return result.ToReturn(login.Error);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        // 用 HttpContext.User.Identity.Name 从  DomainInfoStaticDb 获取 DomainInfo
        public IADResult<DomainInfo> GetLoginDomainInfoUseHttpContext()
        {
            var result = new ADResult<DomainInfo>();
            var name = _httpContext?.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToReturn("HttpContext.User.Name is Null");
            }

            var query = new ADLoginHelper().GetLoginDomainInfo(name);
            if (query.Success)
            {
                return result.ToReturn(query.Value);
            }
            else
            {
                return result.ToReturn(query.Error);
            }
        }
        #endregion    

        #region User Control
        public IADResult<ADUser> EnableUser(string samAccountName)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().EnableAccount(context.Value, samAccountName);
        }

        public IADResult<ADUser> DisableUser(string samAccountName)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().DisableAccount(context.Value, samAccountName);
        }

        public IADResult<ADUser> DeleteUser(string samAccountName)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().DeleteAccount(context.Value, samAccountName);
        }

        public IADResult<List<ADUser>> FindAllUser()
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<List<ADUser>>().ToReturn(context.Error);
            }

            return new ADUserHelper().FindAllUser(context.Value);
        }

        public IADResult<ADUser> FindUser(string samAccountName)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().FindUser(context.Value, samAccountName);
        }

        public IADResult<ADUser> ResetUserPassword(string samAccountName, string newPassword)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().ResetPassword(context.Value, samAccountName, newPassword);
        }

        public IADResult<ADUser> UnlockUser(string samAccountName)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().UnlockAccount(context.Value, samAccountName);
        }

        public IADResult<ADUser> SetUserScriptPath(string samAccountName, string scriptPath)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().SetScriptPath(context.Value, samAccountName, scriptPath);
        }

        public IADResult<ADUser> CreateUser(ADUser newuser, string password)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADUser>().ToReturn(context.Error);
            }

            return new ADUserHelper().CreateUser(context.Value, newuser, password);
        }

        public IADResult<List<ADBase>> FindUserGroups(string samAccount)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<List<ADBase>>().ToReturn(context.Error);
            }

            return new ADUserHelper().FindUserGroups(context.Value, samAccount);
        }

        public IADResult<string> AddUserToGroup(string userSamAccount, List<string> addGroups)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<string>().ToReturn(context.Error);
            }

            return new ADUserHelper().AddUserToGroup(context.Value, userSamAccount,addGroups);
        }
        #endregion

        #region Group Control
        public IADResult<ADGroup> FindGroup(string samAccountName)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADGroup>().ToReturn(context.Error);
            }

            return new ADGroupHelper().FindGroup(context.Value, samAccountName);
        }

        public IADResult<List<ADGroup>> FindALLGroup()
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<List<ADGroup>>().ToReturn(context.Error);
            }

            return new ADGroupHelper().FindALLGroup(context.Value);
        }
        #endregion

        #region Computer Control
        public IADResult<List<ADComputer>> FindAllComputer()
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<List<ADComputer>>().ToReturn(context.Error);
            }

            return new ADComputerHelper().FindAllComputer(context.Value);
        }

        public IADResult<ADComputer> FindComputer(string name)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADComputer>().ToReturn(context.Error);
            }

            return new ADComputerHelper().FindComputer(context.Value,name);
        }

        public IADResult<ADComputer> DeleteComputer(string name)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADComputer>().ToReturn(context.Error);
            }

            return new ADComputerHelper().DeleteComputer(context.Value, name);
        }

        public IADResult<ADComputer> DisableComputer(string name)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADComputer>().ToReturn(context.Error);
            }

            return new ADComputerHelper().DisableComputer(context.Value, name);
        }

        public IADResult<ADComputer> EnableComputer(string name)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADComputer>().ToReturn(context.Error);
            }

            return new ADComputerHelper().EnableComputer(context.Value, name);
        }

        public IADResult<ADComputer> UnlockComputer(string name)
        {
            var context = GetLoginDomainInfoUseHttpContext();
            if (!context.Success)
            {
                return new ADResult<ADComputer>().ToReturn(context.Error);
            }

            return new ADComputerHelper().UnlockComputer(context.Value, name);
        }
        #endregion

        #region DomainInfo Control
        public async Task<IADResult<DomainInfo>> SetDomainInfoToDatabase(DomainInfo info)
        {
            return await new ADDmainInfoHelper().SetDomainInfoToDatabase(_db, info);
        }

        public async Task<IADResult<DomainInfo>> GetDomainInfoInDatabase()
        {
            return await new ADDmainInfoHelper().GetDomainInfo(_db);
        }
        #endregion

    }
}
