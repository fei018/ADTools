using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADServiceLibCore.Models
{
    public class ADBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string SamAccountName { get; set; }

        public string UserPrincipalName { get; set; }

        public string DistinguishedName { get; set; }

        public string StructuralObjectClass { get; set; }

        public bool IsGroup { get; set; }
    }
}
