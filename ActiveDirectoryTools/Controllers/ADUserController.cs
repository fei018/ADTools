using ActiveDirectoryTools.Models;
using ADServiceLibCore;
using ADServiceLibCore.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveDirectoryTools.Controllers
{
    [ValidateAntiForgeryToken]
    public class ADUserController : Controller
    {
        private readonly IADService _adService;

        public ADUserController(IADToolsService service)
        {
            _adService = service.ADService;
        }

        [IgnoreAntiforgeryToken]
        public IActionResult Index()
        {
            return View();
        }

        [IgnoreAntiforgeryToken]
        public IActionResult ADUserIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ADUserFindOne(string samAccount)
        {
            IADResult<ADUser> query = _adService.FindUser(samAccount);
            return PartialView(query);
        }

        [HttpPost]
        public IActionResult ADUserFindAll()
        {
            IADResult<List<ADUser>> query = _adService.FindAllUser();
            return PartialView(query);
        }

        [HttpPost]
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
        public IActionResult ADUserSetScriptPath(string samAccount, string scriptPath)
        {
            var query = _adService.SetUserScriptPath(samAccount, scriptPath?.Trim());
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
        public IActionResult ADUserCreate(ADUser user, string password)
        {

            var query = _adService.CreateUser(user, password);
            if (query.Success)
            {
                return View("ADUserIndex");
            }
            else
            {
                ViewData["ADUserCreate"] = query.Error;
                return View(user);
            }
        }

        [HttpPost]
        public IActionResult ADUserAddGroup(string userSamAccount)
        {
            var query = new ADUserAddGroupsViewModel().GetViewModelList(_adService, userSamAccount);
            return View(query);
        }

        [HttpPost]
        public IActionResult ADUserAddGroup([FromForm] List<string> AddGroup, [FromForm] string userSamAccount)
        {
            var query = _adService.AddUserToGroup(userSamAccount, AddGroup);
            if (query.Success)
            {
                return RedirectToAction("ADUserIndex");
            }
            else
            {
                ViewData["ADUserAddGroup"] = query.Error;
                return ADUserAddGroup(userSamAccount);
            }
        }
    }
}
