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
            try
            {
                if (report == null)
                {
                    var currentAccount = await accountRepository.All().OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();

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
                    var editedAccount = await accountRepository.All().FirstOrDefaultAsync(x => x.Id == report.BlinBerryId);

                    var previousAccount = accountRepository.All().OrderByDescending(x => x.CreatedOn).Skip(1).First();
                    
                    editedAccount.TotalCash = previousAccount.TotalCash + ((model.CountOfKg * 170) - (model.DefectiveKg * 170));
                    editedAccount.Kefir = previousAccount.Kefir - model.TotalKg * 0.55; //литр
                    editedAccount.Oil = previousAccount.Oil - model.TotalKg * 0.074; // литр
                    editedAccount.Salt = previousAccount.Salt - model.TotalKg * 0.0022; // кг
                    editedAccount.Eggs = previousAccount.Eggs - model.TotalKg * 5; // штук
                    editedAccount.Vanila = previousAccount.Vanila - model.TotalKg * 5.3; // грамм
                    editedAccount.Sugar = previousAccount.Sugar - model.TotalKg * 0.06; // кг
                    editedAccount.Soda = previousAccount.Soda - model.TotalKg * 0.16; //грамм

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
