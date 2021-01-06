using ADServiceLibCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceCenter;
using System.Threading.Tasks;
using ActiveDirectoryTools.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace ActiveDirectoryTools.Controllers
{
    [Authorize]    
    public class LoginController : Controller
    {
        private readonly IADService _adService;

        private readonly IConfiguration _configuration;

        public LoginController(IADToolsService toolsService, IConfiguration configuration)
        {
            _adService = toolsService.ADService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await new LoginViewModel().GetDomainInfoList(_configuration);
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewData["LoginError"] = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]       
        public async Task<IActionResult> DoLogin(string username, string password, string domainName)
        {
            var info = await new LoginViewModel().GetSelectedDomainInfo(domainName, _configuration);
            var query = await _adService.Login(info, username, password);
            if (query.Success)
            {
                return RedirectToAction("Index", "ADManage");
            }
            else
            {
                ViewData["LoginError"] = query.Error;
                var vm = await new LoginViewModel().GetDomainInfoList(_configuration);
                return View("Index", vm);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _adService.Logout();
            return RedirectToAction("Index");
        }
    }
}
