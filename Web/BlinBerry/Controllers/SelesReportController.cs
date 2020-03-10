using BlinBerry.Clams;
using BlinBerry.Services.Common.SelesReport;
using BlinBerry.Services.Common.SelesReport.Models;
using GlobalContants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlinBerry.Controllers
{
    public class SelesReportController : Controller
    {
        private readonly ISelesReportService selesReportService;

        public SelesReportController(ISelesReportService selesReportService)
        {
            this.selesReportService = selesReportService;
        }

        public IActionResult Index()
        {
            var reports = selesReportService.GetReportsList();
            return View(reports);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateNewReportAsync(Guid? wordId)
        {
            if (wordId.HasValue)
            {
                var userModel = await selesReportService.PrepeareWordForEditView(wordId);
                ViewBag.IsForEdit = true;
                return this.View(userModel);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewReportAsync(SelesReportDto model)
        {
            await selesReportService.SaveAsync(model, this.User.GetUserId());

            return RedirectToAction("Index", "SelesReport");
        }
    }
}
