﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlinBerry.Clams;
using BlinBerry.Services.Common.ProcurementService;
using BlinBerry.Services.Common.ProcurementService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlinBerry.Controllers
{
    public class ProcurementController : Controller
    {
        private readonly IProcurementService procurementService;

        public ProcurementController(IProcurementService procurementService)
        {
            this.procurementService = procurementService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllProcurements()
        {
            var procurements = procurementService.GetList();
            return View(procurements);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SaveData(Guid? procId)
        {
            if (procId.HasValue)
            {
                var proc = await procurementService.PrepareForEditView(procId);
                return View("Edit", proc);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveData(ProcurementDto model)
        {
            var result = await procurementService.SaveAsync(model, this.User.GetUserId()); 
            return RedirectToAction("GetAllProcurements", "Procurement");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(Guid procId)
        {
            var info = await procurementService.DetailsAsync(procId);
            return View(info);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid procId)
        {
            await procurementService.RemoveAsync(procId);
            return RedirectToAction("GetAllProcurements", "Procurement");
        }
    }
}
