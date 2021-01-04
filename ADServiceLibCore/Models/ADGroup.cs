using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace ADServiceLibCore.Models
{
    public class ADGroup : ADBase
    {
        public ADGroup()
        {
            base.IsGroup = true;
        }

        public List<ADBase> Members { get; set; }

        public ADGroup NewCopy(GroupPrincipal group)
        {
            var newGroup = new ADGroup
            {
                Name = group.Name,
                DisplayName = group.DisplayName,
                DistinguishedName = group.DistinguishedName,
                SamAccountName = group.SamAccountName,
                UserPrincipalName = group.UserPrincipalName,
                Description = group.Description,
                Members = new List<ADBase>()
            };

            try
            {
                var membs = group.GetMembers().ToList();
                if (membs != null && membs.Any())
                {
                    membs.ForEach(m =>
                    {
                        if (m is UserPrincipal u) newGroup.Members.Add(new ADUser().NewCopy(u));

                        if (m is GroupPrincipal) newGroup.Members.Add(NewADBase(m));
                    });
                }
            }
            catch (Exception)
            {
                // 没权限递归成员会抛出异常 
                // ErrorCode=-2147016661 , InnerException.ErrorCode=-2147016661, InnerException.HResult=-2147016661
                //if (ex.HResult.Equals(-2146233087)) return newGroup;
                return newGroup;
            }

            return newGroup;
        }

        public ADBase NewADBase(Principal g)
        {
            var b = new ADBase
            {
                Description = g.Description,
                DisplayName = g.DisplayName,
                DistinguishedName = g.DistinguishedName,
                Name = g.Name,
                SamAccountName = g.SamAccountName,
                UserPrincipalName = g.UserPrincipalName,
                IsGroup = true
            };
            return b;
        }
    }
}
