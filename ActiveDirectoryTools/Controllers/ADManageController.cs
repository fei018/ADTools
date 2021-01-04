using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveDirectoryTools.Models;
using ADServiceLibCore;
using ADServiceLibCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceCenter;

namespace ActiveDirectoryTools.Controllers
{
    [Authorize]   
    public class ADManageController : Controller
    {
        private readonly IADService _adService;

        public ADManageController(IADToolsService service)
        {
            _adService = service.ADService;
        }

        public IActionResult Index()
        {
            var vm = new ADManageIndexViewModel();

            var domain = _adService.GetLoginDomainInfoUseHttpContext();
            if (domain.Success)
            {
                vm.DomainName = domain.Value.DomainName;
                vm.ConnectServer = domain.Value.ConnectedServer;
                vm.LoginName = domain.Value.AdminName;
            }
            //var user = _adService.FindAllUser();
            //if (user.Success)
            //{
            //    vm.ADUserCount = user.Value.Count;
            //}
            //var computer = _adService.FindAllComputer();
            //if (computer.Success)
            //{
            //    vm.ADComputerCount = computer.Value.Count;
            //}
            //var group = _adService.FindALLGroup();
            //if (group.Success)
            //{
            //    vm.ADGroupCount = group.Value.Count;
            //}
            return View(vm);
        }

        #region User
        public IActionResult ADUserIndex()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserFindOne(string samAccount)
        {
            IADResult<ADUser> query = _adService.FindUser(samAccount);
            return PartialView(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserFindAll()
        {
            IADResult<List<ADUser>> query = _adService.FindAllUser();
            return PartialView(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserUnlock(string samAccount)
        {
            IADResult<ADUser> query = _adService.UnlockUser(samAccount);
            if (query.Success)
            {
                return Content(samAccount + " Action Done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserEnable(string samAccount)
        {
            var query = _adService.EnableUser(samAccount);
            if (query.Success)
            {
                return Content(samAccount + " Action Done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserDisable(string samAccount)
        {
            var query = _adService.DisableUser(samAccount);
            if (query.Success)
            {
                return Content(samAccount + " Action Done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserResetPassword(string samAccount, string newPassword)
        {
            var query = _adService.ResetUserPassword(samAccount, newPassword?.Trim());
            if (query.Success)
            {
                return Content(samAccount + " Action Done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserDelete(string samAccount)
        {
            var query = _adService.DeleteUser(samAccount);
            if (query.Success)
            {
                return Content(samAccount + " Action Done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserSetScriptPath(string samAccount,string scriptPath)
        {
            var query = _adService.SetUserScriptPath(samAccount,scriptPath?.Trim());
            if (query.Success)
            {
                return Content(samAccount + " Action Done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        public IActionResult ADUserCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserCreate(ADUser user, string password)
        {

            var query = _adService.CreateUser(user, password);
            if (query.Success)
            {
                ViewData["ADUserCreate"] = "Domain User Create Success.";
                return View();
            }
            else
            {
                ViewData["ADUserCreate"] = query.Error;
                return View(user);
            }
        }


        public IActionResult ADUserAddGroup(string userSamAccount)
        {
            var query = new ADUserAddGroupsViewModel().GetViewModelList(_adService, userSamAccount);
            return View(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADUserAddGroup([FromForm]List<string> AddGroup, [FromForm]string userSamAccount)
        {
            var query = _adService.AddUserToGroup(userSamAccount, AddGroup);
            if (query.Success)
            {
                return ADUserAddGroup(query.Value);
            }
            return View();
        }
        #endregion

        #region Group
        public IActionResult ADGroupIndex()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADGroupFindOne(string groupName)
        {
            var query = _adService.FindGroup(groupName);
            return PartialView(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADGroupFindAll()
        {
            IADResult<List<ADGroup>> query = _adService.FindALLGroup();
            return PartialView(query);
        }

        #endregion

        #region Computer
        public IActionResult ADComputerIndex()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADComputerFindAll()
        {
            IADResult<List<ADComputer>> query = _adService.FindAllComputer();
            return PartialView(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADComputerFindOne(string name)
        {
            var query = _adService.FindComputer(name);
            return PartialView(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADComputerDelete(string name)
        {
            var query = _adService.DeleteComputer(name);
            if (query.Success)
            {
                return Content(name + " action done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADComputerDisable(string name)
        {
            var query = _adService.DisableComputer(name);
            if (query.Success)
            {
                return Content(name + " action done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADComputerEnable(string name)
        {
            var query = _adService.EnableComputer(name);
            if (query.Success)
            {
                return Content(name + " action done.");
            }
            else
            {
                return Content(query.Error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ADComputerUnlock(string name)
        {
            var query = _adService.UnlockComputer(name);
            if (query.Success)
            {
                return Content(name + " action done.");
            }
            else
            {
                return Content(query.Error);
            }
        }
        #endregion
    }
}
