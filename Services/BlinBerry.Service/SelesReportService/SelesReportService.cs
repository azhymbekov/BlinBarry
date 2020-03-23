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

        public IQueryable<SelesReportDto> GetReportsList()
        {
            return from r in reportRepository.All().OrderByDescending(x => x.DayOfWeek)
                   select new SelesReportDto
                   {
                       Id = r.Id,
                       CountOfKg = r.CountOfKg ,
                       DayOfWeek = r.DayOfWeek,
                       DefectiveKg = r.DefectiveKg,
                       Cash = r.CountOfKg*170
                   };
        }

        public async Task<SelesReportDto> PrepeareWordForEditView(Guid? id)
        {
            var report = (from w in reportRepository.AllAsNoTracking()
                        where w.Id == id
                        select new SelesReportDto()
                        {
                            Id = w.Id,
                            CountOfKg = w.CountOfKg,
                            DayOfWeek = w.DayOfWeek,
                            DefectiveKg = w.DefectiveKg
                        }).FirstOrDefault();
            return report;
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> SaveAsync(SelesReportDto model, Guid userId)
        {
            var result = new OperationResult();

            var report = await reportRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

            var currentAccount = await accountRepository.All().OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            try
            {
                if (report == null)
                {                  

                    var newReport = new SelesReport
                    {
                        Id = Guid.NewGuid(),
                        CountOfKg = model.CountOfKg,
                        DefectiveKg = model.DefectiveKg,
                        DayOfWeek = model.DayOfWeek
                    };
                    
                    var newTransaction = new CommonMoneyAndProducts()
                    {
                        Id = Guid.NewGuid(),
                        TotalCash = currentAccount.TotalCash + ((model.CountOfKg * 170) - (model.DefectiveKg * 170)),
                        Kefir = currentAccount.Kefir - model.TotalKg * 0.55, //литр
                        Oil = currentAccount.Oil - model.TotalKg * 0.074, // литр
                        Salt = currentAccount.Salt - model.TotalKg * 0.0022, // кг
                        Eggs = currentAccount.Eggs - model.TotalKg * 5, // штук
                        Vanila = currentAccount.Vanila - model.TotalKg * 5.3, // грамм
                        Sugar = currentAccount.Sugar - model.TotalKg * 0.06, // кг
                        Soda = currentAccount.Soda - model.TotalKg * 0.16, //грамм
                    };

                    newReport.BlinBerryId = newTransaction.Id;
                    reportRepository.Add(newReport);
                    accountRepository.Add(newTransaction);
                }

                else
                {                    
                    //za4em dobavlyat ewe i braki oni ved ne vliyayut na kassy
                    currentAccount.TotalCash += ((model.CountOfKg * 170) - (report.CountOfKg * 170));
                    currentAccount.Kefir -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 0.55; //литр
                    currentAccount.Oil -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 0.074; // литр
                    currentAccount.Salt -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 0.0022; // кг
                    currentAccount.Eggs -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 5; // штук
                    currentAccount.Vanila -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 5.3; // грамм
                    currentAccount.Sugar -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 0.06; // кг
                    currentAccount.Soda -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * 0.16; //грамм

                    report.DayOfWeek = model.DayOfWeek;
                    report.CountOfKg = model.CountOfKg;
                    report.DefectiveKg = model.DefectiveKg;
                }

                await reportRepository.SaveChangesAsync(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            return result;
        }
    }
}
