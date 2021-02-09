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
    public class ADGroupController : Controller
    {
        private readonly IADService _adService;

        public ADGroupController(IADToolsService service)
        {
            _adService = service.ADService;
        }

        [IgnoreAntiforgeryToken]
        public IActionResult Index()
        {
            return View();
        }

        [IgnoreAntiforgeryToken]
        public IActionResult ADGroupIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ADGroupFindOne(string groupName)
        {
            var query = _adService.FindGroup(groupName);
            return PartialView(query);
        }

        [HttpPost]
        public IActionResult ADGroupFindAll()
        {
            IADResult<List<ADGroup>> query = _adService.FindALLGroup();
            return PartialView(query);
        }
    }
}
