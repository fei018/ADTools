using ADServiceLibCore.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Collections;

namespace ADServiceLibCore.Services
{
    internal class ADGroupHelper
    {
        public IADResult<ADGroup> FindGroup(DomainInfo info, string samAccountName)
        {
            var result = new ADResult<ADGroup>();

            if (string.IsNullOrWhiteSpace(samAccountName))
            {
                return result.ToReturn("group name Is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using GroupPrincipal group = new GroupPrincipal(context, samAccountName);
                using var g = new PrincipalSearcher(group).FindOne() as GroupPrincipal;
                
                if (g != null)
                {
                    return result.ToReturn(new ADGroup().NewCopy(g));
                }

                return result.ToReturn("Cannot find the groupname in "+context.Name);
            }
            catch (Exception ex)
            {               
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<List<ADGroup>> FindALLGroup(DomainInfo info)
        {
            var result = new ADResult<List<ADGroup>>();
            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using var groupPrin = new GroupPrincipal(context);
                using var searchGroups = new PrincipalSearcher(groupPrin).FindAll();

                var list = new List<ADGroup>();

                var principals = searchGroups.ToList();
                if (principals != null && principals.Any())
                {
                    foreach (var p in principals)
                    {
                        if (p is GroupPrincipal g)
                        {
                            list.Add(new ADGroup().NewCopy(g));
                        }
                    }
                    return result.ToReturn(list);
                }

                return result.ToReturn("Cannot find any AD Groups in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }
    }
}
