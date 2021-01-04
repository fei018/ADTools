using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveDirectoryTools.Models
{
    public class ADManageIndexViewModel
    {
        public string DomainName { get; set; }

        public string ConnectServer { get; set; }

        public string LoginName { get; set; }

        public int ADUserCount { get; set; }

        public int ADComputerCount { get; set; }

        public int ADGroupCount { get; set; }
    }
}
