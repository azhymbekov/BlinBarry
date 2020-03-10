using BlinBerry.Data.Models.Entities;
using BlinBerry.Data.Repositories;
using BlinBerry.Services.Common.SelesReport;
using BlinBerry.Services.Common.SelesReport.Models;
using GlobalContants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinBerry.Service.SelesReportService
{
    public class SelesReportService : ISelesReportService
    {
        private readonly IRepository<SelesReport> reportRepository;

        private readonly IRepository<CommonMoneyAndProducts> accountRepository;

        public SelesReportService(IRepository<SelesReport> reportRepository,
             IRepository<CommonMoneyAndProducts> accountRepository)
        {
            this.reportRepository = reportRepository;
            this.accountRepository = accountRepository;
        }

        public Task<SelesReportDto> GetReportAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<SelesReportDto> GetReportsList()
        {
            return from r in reportRepository.All()
                   select new SelesReportDto
                   {
                       Id = r.Id,
                       CountOfKg = r.CountOfKg ,
                       DayOfWeek = r.DayOfWeek,
                       DefectiveKg = r.DefectiveKg,
                       
                   };
        }

        public Task<SelesReportDto> PrepeareWordForEditView(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> SaveAsync(SelesReportDto model, Guid userId)
        {
            var result = new OperationResult();

            var report = await reportRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

            var account = await accountRepository.All().FirstOrDefaultAsync();

            if (report == null)
            {
                var newReport = new SelesReport
                {
                    Id = Guid.NewGuid(),
                    CountOfKg = model.CountOfKg,
                    DefectiveKg = model.DefectiveKg,
                    DayOfWeek = model.DayOfWeek
                };
                reportRepository.Add(newReport);
            }            
                        
            else
            {
                report.DayOfWeek = model.DayOfWeek;
                report.CountOfKg = model.CountOfKg;
                report.DefectiveKg = model.DefectiveKg;
            }

            account.TotalCash = account.TotalCash + ((model.CountOfKg * 170) - (model.DefectiveKg * 170));
            account.Kefir = account.Kefir - model.TotalKg * 0.55; //литр
            account.Oil = account.Oil - model.TotalKg * 0.074; // литр
            account.Salt = account.Salt -model.TotalKg * 0.0022; // кг
            account.Eggs = account.Eggs - model.TotalKg * 5; // штук
            account.Vanila = account.Vanila - model.TotalKg * 5.3; // грамм
            account.Sugar = account.Sugar - model.TotalKg * 0.06; // кг
            account.Soda = account.Soda - model.TotalKg * 0.16; //грамм

            
            await reportRepository.SaveChangesAsync(userId);
            return result;
        }
    }
}
