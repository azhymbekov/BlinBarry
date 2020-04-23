using BlinBerry.Clams;
using BlinBerry.Services.Common.SelesReport;
using BlinBerry.Services.Common.SelesReport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> CreateNewReportAsync(Guid? seleId)
        {
            if (seleId.HasValue)
            {
                var userModel = await selesReportService.PrepareWordForEditView(seleId);
                return this.View(userModel);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewReportAsync(SeleTransactionDto model)
        {
            await selesReportService.SaveAsync(model, this.User.GetUserId());
            return RedirectToAction("Index", "SelesReport");
        }


        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await selesReportService.RemoveAsync(id);
            return RedirectToAction("Index", "SelesReport");
        }

    }
}
