using ADServiceLibCore.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace ADServiceLibCore.Services
{
    internal class ADComputerHelper
    {
        public IADResult<ADComputer> EnableComputer(DomainInfo info, string name)
        {
            var result = new ADResult<ADComputer>();
            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToReturn("computer Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using ComputerPrincipal comPrin = new ComputerPrincipal(context);
                comPrin.Name = name;

                using var com = (new PrincipalSearcher(comPrin).FindOne()) as ComputerPrincipal;

                if (com != null)
                {
                    com.Enabled = true;
                    com.Save();
                    return result.ToReturn(new ADComputer().NewCopy(com));
                }
                return result.ToReturn("Cannot find the computer in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADComputer> DisableComputer(DomainInfo info, string name)
        {
            var result = new ADResult<ADComputer>();
            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToReturn("computer Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using ComputerPrincipal comPrin = new ComputerPrincipal(context);
                comPrin.Name = name;

                using var com = (new PrincipalSearcher(comPrin).FindOne()) as ComputerPrincipal;

                if (com != null)
                {
                    com.Enabled = false;
                    com.Save();
                    return result.ToReturn(new ADComputer().NewCopy(com));
                }
                return result.ToReturn("Cannot find the computer in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADComputer> UnlockComputer(DomainInfo info, string name)
        {
            var result = new ADResult<ADComputer>();
            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToReturn("computer Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using ComputerPrincipal comPrin = new ComputerPrincipal(context);
                comPrin.Name = name;

                using var com = (new PrincipalSearcher(comPrin).FindOne()) as ComputerPrincipal;

                if (com != null)
                {
                    if (com.IsAccountLockedOut())
                    {
                        com.UnlockAccount();
                        com.Save();
                    }
                    return result.ToReturn(new ADComputer().NewCopy(com));
                }
                return result.ToReturn("Cannot find the computer in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADComputer> DeleteComputer(DomainInfo info, string name)
        {
            var result = new ADResult<ADComputer>();

            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToReturn("computer Name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using ComputerPrincipal comPrin = new ComputerPrincipal(context);
                comPrin.Name = name;

                using var com = (new PrincipalSearcher(comPrin).FindOne()) as ComputerPrincipal;

                if (com != null)
                {
                    var copy = new ADComputer().NewCopy(com);

                    com.Delete();

                    return result.ToReturn(copy);
                }
                return result.ToReturn("Cannot find the computer in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<ADComputer> FindComputer(DomainInfo info, string name)
        {
            var result = new ADResult<ADComputer>();
            if (string.IsNullOrWhiteSpace(name))
            {
                return result.ToReturn("computer name is NullOrWhiteSpace");
            }

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using ComputerPrincipal comPrin = new ComputerPrincipal(context);
                comPrin.Name = name;

                using var com = (new PrincipalSearcher(comPrin).FindOne()) as ComputerPrincipal;

                if (com != null)
                {
                    return result.ToReturn(new ADComputer().NewCopy(com));
                }
                return result.ToReturn("Cannot find the computer in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }

        public IADResult<List<ADComputer>> FindAllComputer(DomainInfo info)
        {
            var result = new ADResult<List<ADComputer>>();

            try
            {
                using var context = new PrincipalContext(ContextType.Domain, info.DomainName, info.ContainerString, info.AdminName, info.AdminPassword);
                using ComputerPrincipal comPrincipal = new ComputerPrincipal(context);
                using var principals = new PrincipalSearcher(comPrincipal).FindAll();

                var newList = new List<ADComputer>();

                if (principals != null && principals.Any())
                {
                    foreach (var p in principals)
                    {
                        if (p is ComputerPrincipal c)
                        {
                            newList.Add(new ADComputer().NewCopy(c));
                        }
                    }
                    return result.ToReturn(newList);
                }

                return result.ToReturn("Cannot find any computer in " + context.Name);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }
    }
}
