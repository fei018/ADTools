using ADServiceLibCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADServiceLibCore
{
    public interface IADService
    {
        Task<IADResult<DomainInfo>> Login(DomainInfo domain, string loginName, string password);

        Task Logout();

        IADResult<DomainInfo> GetLoginDomainInfoUseHttpContext();

        // User control
        IADResult<ADUser> FindUser(string samAccountName);

        IADResult<List<ADUser>> FindAllUser();

        IADResult<ADUser> EnableUser(string samAccountName);

        IADResult<ADUser> DisableUser(string samAccountName);

        IADResult<ADUser> DeleteUser(string samAccountName);

        IADResult<ADUser> UnlockUser(string samAccountName);

        IADResult<ADUser> ResetUserPassword(string samAccountName, string newPassword);

        IADResult<ADUser> SetUserScriptPath(string samAccountName, string scriptPath);

        IADResult<ADUser> CreateUser(ADUser newuser, string password);

        IADResult<List<ADBase>> FindUserGroups(string samAccount);

        IADResult<string> AddUserToGroup(string userSamAccount, List<string> addGroups);

        // Group Control

        IADResult<ADGroup> FindGroup(string groupName);

        IADResult<List<ADGroup>> FindALLGroup();

        // Computer
        IADResult<List<ADComputer>> FindAllComputer();

        IADResult<ADComputer> FindComputer(string name);

        IADResult<ADComputer> DeleteComputer(string name);

        IADResult<ADComputer> DisableComputer(string name);

        IADResult<ADComputer> EnableComputer(string name);

        IADResult<ADComputer> UnlockComputer(string name);

        // DomainInfo
        //Task<IADResult<DomainInfo>> SetDomainInfoToDatabase(DomainInfo info);

        //Task<IADResult<DomainInfo>> GetDomainInfoInDatabase();
    }
}
