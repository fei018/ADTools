using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADServiceLibCore.Models;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace ADServiceLibCore.Services
{
    internal class ADUserHelper
    {
        public IADResult<ADUser> EnableAccount(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<ADUser>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;

                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    user.Enabled = true;
                    user.Save();
                    return result.ToReturn(new ADUser().NewCopy(user));
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> DisableAccount(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<ADUser>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;

                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    user.Enabled = false;
                    user.Save();
                    return result.ToReturn(new ADUser().NewCopy(user));
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> UnlockAccount(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<ADUser>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;

                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    if (user.IsAccountLockedOut())
                    {
                        user.UnlockAccount();
                        user.Save();
                    }
                    return result.ToReturn(new ADUser().NewCopy(user));
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> ResetPassword(DomainInfo info, string samAccountName, string newPassword)
        {
            var result = new ADResult<ADUser>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;
                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    user.SetPassword(newPassword);
                    user.Save();
                    return result.ToReturn(new ADUser().NewCopy(user));
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return result.ToReturn(ex.InnerException.Message);
                }
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> SetScriptPath(DomainInfo info, string samAccountName, string scriptPath)
        {
            var result = new ADResult<ADUser>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;
                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    user.ScriptPath = scriptPath;
                    user.Save();
                    return result.ToReturn(new ADUser().NewCopy(user));
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return result.ToReturn(ex.InnerException.Message);
                }
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> DeleteAccount(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<ADUser>();

            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;

                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    var copy = new ADUser().NewCopy(user);

                    user.Delete();

                    return result.ToReturn(copy);
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> FindUser(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<ADUser>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;

                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    return result.ToReturn(new ADUser().NewCopy(user));
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<List<ADUser>> FindAllUser(DomainInfo info)
        {
            var result = new ADResult<List<ADUser>>();

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrincipal = new UserPrincipal(context);
                using var principals = new PrincipalSearcher(userPrincipal).FindAll();

                var newList = new List<ADUser>();

                if (principals != null && principals.Any())
                {
                    foreach (var p in principals)
                    {
                        if (p is UserPrincipal u)
                        {
                            newList.Add(new ADUser().NewCopy(u));
                        }
                    }
                    return result.ToReturn(newList);
                }

                return result.ToReturn("Cannot find any AD User in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADUser> CreateUser(DomainInfo info, ADUser newuser, string password)
        {
            var result = new ADResult<ADUser>();
            if (newuser == null || string.IsNullOrWhiteSpace(newuser.SamAccountName))
            {
                return result.ToReturn("User is NullOrWhiteSpace");
            }

            try
            {
                var exist = FindUser(info, newuser.SamAccountName);
                if (exist.Success && exist.Value.Name == newuser.Name)
                {
                    return result.ToReturn("Logon Name or Full Name is exist");
                }

                var container = "cn=users," + info.ContainerString;
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, container, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                // set value
                userPrin.SamAccountName = newuser.SamAccountName;
                userPrin.Surname = newuser.Surname;
                userPrin.GivenName = newuser.GivenName;
                userPrin.Name = newuser.Name;
                userPrin.DisplayName = newuser.DisplayName;
                userPrin.Description = newuser.Description;
                userPrin.UserPrincipalName = newuser.SamAccountName + "@" + info.DomainName;

                userPrin.Save();
                userPrin.Enabled = true;
                userPrin.SetPassword(password);
                userPrin.Save();

                return result.ToReturn(new ADUser().NewCopy(userPrin));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return result.ToReturn(ex.InnerException.Message);
                }
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<List<ADBase>> FindUserGroups(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<List<ADBase>>();
            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("User Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using UserPrincipal userPrin = new UserPrincipal(context);
                userPrin.SamAccountName = samAccountName;

                using var user = (new PrincipalSearcher(userPrin).FindOne()) as UserPrincipal;

                if (user != null)
                {
                    using var gs = user.GetGroups(context);
                    var list = new List<ADBase>();
                    foreach (var g in gs)
                    {
                        list.Add(new ADGroup().NewADBase(g));
                    }
                    return result.ToReturn(list);
                }
                return result.ToReturn("Cannot find the username in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<string> AddUserToGroup(DomainInfo info, string userSamAccount, List<string> addGroups)
        {
            var result = new ADResult<string>();

            var queryUserGroup = FindUserGroups(info, userSamAccount);
            if (!queryUserGroup.Success)
            {
                return result.ToReturn(queryUserGroup.Error);
            }

            var userGroups = queryUserGroup.Value;
            try
            {
                foreach (var add in addGroups)
                {
                    // 要添加的组 不存在 本来组里面
                    if (!userGroups.Any(ug => ug.SamAccountName == add))
                    {
                        using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                        using GroupPrincipal groupPrin = new GroupPrincipal(context);
                        groupPrin.SamAccountName = add;
                        using var find = (new PrincipalSearcher(groupPrin).FindOne()) as GroupPrincipal;
                        find.Members.Add(context, IdentityType.SamAccountName, userSamAccount);
                        find.Save();
                    }
                }

                foreach (var ug in userGroups)
                {
                    if (!addGroups.Contains(ug.SamAccountName))
                    {
                        using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                        using GroupPrincipal groupPrin = new GroupPrincipal(context);
                        groupPrin.SamAccountName = ug.SamAccountName;
                        using var find = (new PrincipalSearcher(groupPrin).FindOne()) as GroupPrincipal;
                        find.Members.Remove(context, IdentityType.SamAccountName, userSamAccount);
                        find.Save();
                    }
                }

                return result.ToReturn(true, userSamAccount);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        //public IADResult<ADUser> RemoveUserInGroup(string groupName, string userSamAccount)
        //{
        //    var result = new ADResult<ADUser>();
        //    return result;
        //}
    }
}
