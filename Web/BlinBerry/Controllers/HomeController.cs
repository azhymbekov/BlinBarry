using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlinBerry.Models;
using BlinBerry.Services.Common.CommonInfoService;
using Microsoft.AspNetCore.Authorization;

namespace BlinBerry.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICommonInfoAboutAccountService infoService;

        public HomeController(ICommonInfoAboutAccountService infoService)
        {
            this.infoService = infoService;
        }

        public double? GetAccountInfo()
        {
            return infoService.GetAccountInfo();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var info = await infoService.GetCommonInfo();
            return this.View(info);
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
