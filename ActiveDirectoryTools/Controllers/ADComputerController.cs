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
    public class ADComputerController : Controller
    {
        private readonly IADService _adService;

        public ADComputerController(IADToolsService service)
        {
            _adService = service.ADService;
        }

        [IgnoreAntiforgeryToken]
        public IActionResult Index()
        {
            return View();
        }

        [IgnoreAntiforgeryToken]
        public IActionResult ADComputerIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ADComputerFindAll()
        {
            IADResult<List<ADComputer>> query = _adService.FindAllComputer();
            return PartialView(query);
        }

        [HttpPost]
        public IActionResult ADComputerFindOne(string name)
        {
            var query = _adService.FindComputer(name);
            return PartialView(query);
        }

        [HttpPost]
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
    }
}
