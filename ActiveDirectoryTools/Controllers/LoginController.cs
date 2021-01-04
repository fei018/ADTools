using ADServiceLibCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceCenter;
using System.Threading.Tasks;

namespace ActiveDirectoryTools.Controllers
{
    [Authorize]    
    public class LoginController : Controller
    {
        private readonly IADService _adService;

        public LoginController(IADToolsService toolsService)
        {
            _adService = toolsService.ADService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]       
        public async Task<IActionResult> DoLogin(string username, string password)
        {
            ViewData["LoginError"] = null;

            var query = await _adService.Login(loginName: username, password);
            if (query.Success)
            {
                return RedirectToAction("Index", "ADManage");
            }
            else
            {
                ViewData["LoginError"] = query.Error;
                return View("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _adService.Logout();
            return RedirectToAction("Index");
        }
    }
}
