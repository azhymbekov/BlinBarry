using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlinBerry.Clams;
using BlinBerry.Services.Common.ProcurementService.Models;
using BlinBerry.Services.Common.SpendingService;
using BlinBerry.Services.Common.SpendingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlinBerry.Controllers
{
    public class SpendingController :Controller
    {
        private readonly ISpendingService spendingService;

        public SpendingController(ISpendingService spendingService)
        {
            this.spendingService = spendingService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllSpendings()
        {
            var spends = spendingService.GetList();
            return View(spends);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SaveData(Guid? Id)
        {
            if (Id.HasValue)
            {
                var proc = await spendingService.PrepareForEditView(Id);
                return this.View(proc);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveData(SpendingDto model)
        {
            var result = await spendingService.SaveAsync(model, this.User.GetUserId());
            return RedirectToAction("GetAllSpendings", "Spending");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(Guid Id)
        {
            var details = await spendingService.DetailsAsync(Id);
            return View(details);
        }

    }
}
