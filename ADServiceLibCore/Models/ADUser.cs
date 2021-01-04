using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace ADServiceLibCore.Models
{
    public class ADUser : ADBase
    {
        public ADUser()
        {
            base.IsGroup = false;
        }

        public bool LockedOut { get; set; }

        public bool Enabled { get; set; }

        public string ScriptPath { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public ADUser NewCopy(UserPrincipal user)
        {
            var aduser = new ADUser
            {
                Name = user.Name,
                DisplayName = user.DisplayName,
                SamAccountName = user.SamAccountName,
                UserPrincipalName = user.UserPrincipalName,
                DistinguishedName = user.DistinguishedName,
                Description = user.Description,
                StructuralObjectClass = user.StructuralObjectClass,
                LockedOut = user.IsAccountLockedOut(),
                ScriptPath = user.ScriptPath,
                GivenName = user.GivenName,
                Surname = user.Surname,
                Enabled = user.Enabled ?? false
            };

            return aduser;
        }
    }
}
