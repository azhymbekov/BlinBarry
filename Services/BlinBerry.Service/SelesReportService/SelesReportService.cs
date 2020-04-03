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
        private readonly IRepository<SeleTransaction> reportRepository;

        private readonly IRepository<State> accountRepository;

        private readonly IRepository<Recipe> recipeRepository;

        public SelesReportService(IRepository<SeleTransaction> reportRepository,
             IRepository<State> accountRepository,
             IRepository<Recipe> recipeRepository)
        {
            this.reportRepository = reportRepository;
            this.accountRepository = accountRepository;
            this.recipeRepository = recipeRepository;
        }

        public IQueryable<SeleTransactionDto> GetReportsList()
        {
            return from r in reportRepository.All().OrderByDescending(x => x.Date)
                   select new SeleTransactionDto
                   {
                       Id = r.Id,
                       CountOfKg = r.CountOfKg ,
                       DayOfWeek = r.Date,
                       DefectiveKg = r.DefectiveKg,
                       TotalProfit = r.TotalProfit
                   };
        }

        public async Task<SeleTransactionDto> PrepeareWordForEditView(Guid? id)
        {
            var report = (from w in reportRepository.AllAsNoTracking()
                        where w.Id == id
                        select new SeleTransactionDto()
                        {
                            Id = w.Id,
                            CountOfKg = w.CountOfKg,
                            DayOfWeek = w.Date,
                            DefectiveKg = w.DefectiveKg,
                            TotalProfit = w.TotalProfit
                        }).FirstOrDefault();
            return report;
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> SaveAsync(SeleTransactionDto model, Guid userId)
        {
            var result = new OperationResult();

            var report = await reportRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

            //если добавятся другие рецепты , то у SeleTransaction добавиться новое поле 

            var recipe = await recipeRepository.All().FirstOrDefaultAsync();

            var currentAccount = await accountRepository.All().OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            try
            {
                if (report == null)
                {                  

                    var newReport = new SeleTransaction
                    {
                        Id = Guid.NewGuid(),
                        CountOfKg = model.CountOfKg,
                        DefectiveKg = model.DefectiveKg,
                        Date = model.DayOfWeek,
                        TotalProfit = model.TotalProfit
                    };

                    currentAccount.TotalCash += model.TotalProfit;
                    currentAccount.Kefir -= model.TotalKg * recipe.Kefir;
                    currentAccount.Oil -= model.TotalKg * recipe.Oil;
                    currentAccount.Salt -= model.TotalKg * recipe.Salt;
                    currentAccount.Eggs -= model.TotalKg * recipe.Eggs;
                    currentAccount.Vanila -= model.TotalKg * recipe.Vanila;
                    currentAccount.Sugar -= model.TotalKg * recipe.Sugar;
                    currentAccount.Soda -= model.TotalKg * recipe.Soda;
                   
                    reportRepository.Add(newReport);
                }

                else
                {                    
                    //za4em dobavlyat ewe i braki oni ved ne vliyayut na kassy
                    currentAccount.TotalCash += (model.TotalProfit - report.TotalProfit);
                    currentAccount.Kefir -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Kefir; //литр
                    currentAccount.Oil -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Oil; // литр
                    currentAccount.Salt -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Salt; // кг
                    currentAccount.Eggs -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Eggs; // штук
                    currentAccount.Vanila -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Vanila; // грамм
                    currentAccount.Sugar -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Sugar; // кг
                    currentAccount.Soda -= (model.TotalKg - (report.CountOfKg + report.DefectiveKg)) * recipe.Soda; //грамм

                    report.Date = model.DayOfWeek;
                    report.CountOfKg = model.CountOfKg;
                    report.DefectiveKg = model.DefectiveKg;
                    report.TotalProfit = model.TotalProfit;
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
