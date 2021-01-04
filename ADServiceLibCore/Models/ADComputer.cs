using System.DirectoryServices.AccountManagement;

namespace ADServiceLibCore.Models
{
    public class ADComputer : ADBase
    {
        public ADComputer()
        {
            base.IsGroup = false;
        }

        public bool LockedOut { get; set; }

        public bool Enabled { get; set; }

        public ADComputer NewCopy(ComputerPrincipal computer)
        {
            var adComputer = new ADComputer
            {
                Name = computer.Name,
                DisplayName = computer.DisplayName,
                SamAccountName = computer.SamAccountName,
                UserPrincipalName = computer.UserPrincipalName,
                DistinguishedName = computer.DistinguishedName,
                Description = computer.Description,
                StructuralObjectClass = computer.StructuralObjectClass,
                LockedOut = computer.IsAccountLockedOut(),
                Enabled = computer.Enabled ?? false
            };

            return adComputer;
        }
    }
}
