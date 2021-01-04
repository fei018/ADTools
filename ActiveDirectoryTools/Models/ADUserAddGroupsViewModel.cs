using ADServiceLibCore;
using ADServiceLibCore.Models;
using ADServiceLibCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveDirectoryTools.Models
{
    public class ADUserAddGroupsViewModel
    {
        public string UserSamAccount { get; set; }

        public bool IsCheck { get; set; }

        public string GroupName { get; set; }

        public List<ADUserAddGroupsViewModel> GetViewModelList(IADService service, string userSamAccount)
        {
            List<ADBase> userGroups =null;
            var query = service.FindUserGroups(userSamAccount);
            if (query.Success)
            {
                userGroups = query.Value;
            }

            List<ADGroup> allgroup =null;
            var query2 = service.FindALLGroup();
            if (query2.Success)
            {
                allgroup = query2.Value;
            }

            var list = new List<ADUserAddGroupsViewModel>();
            if (userGroups != null && allgroup != null)
            {               
                foreach (var g in allgroup)
                {
                    if(userGroups.Any(u => u.SamAccountName == g.SamAccountName))
                    {
                        list.Add(new ADUserAddGroupsViewModel { GroupName = g.SamAccountName, IsCheck = true, UserSamAccount = userSamAccount });
                    }
                    else
                    {
                        list.Add(new ADUserAddGroupsViewModel { GroupName = g.SamAccountName, IsCheck = false, UserSamAccount = userSamAccount });
                    }                   
                }
            }

            return list;
        }
    }
}
