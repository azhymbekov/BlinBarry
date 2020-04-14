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
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace BlinBerry.Service.SelesReportService
{
    public class SelesReportService : ISelesReportService
    {
        private readonly IRepository<SeleTransaction> reportRepository;

        private readonly IRepository<State> accountRepository;

        private readonly IRepository<Recipe> recipeRepository;

        private readonly IMapper mapper;

        private readonly ILogger<SelesReportService> logger;

        public SelesReportService(IRepository<SeleTransaction> reportRepository,
             IRepository<State> accountRepository,
             IRepository<Recipe> recipeRepository, IMapper mapper,
             ILogger<SelesReportService> logger)
        {
            this.reportRepository = reportRepository;
            this.accountRepository = accountRepository;
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IQueryable<SeleTransactionDto> GetReportsList()
        {
            return from r in reportRepository.All().OrderByDescending(x => x.Date)
                   select mapper.Map<SeleTransactionDto>(r);
        }

        public async Task<SeleTransactionDto> PrepareWordForEditView(Guid? id)
        {
            var report = await reportRepository.GetByAsync(x => x.Id == id);
                       
            return mapper.Map<SeleTransactionDto>(report);
        }

        public async Task RemoveAsync(Guid id)
        {
            var report = await reportRepository.GetByIdAsync(id);
            var commonAccount = await accountRepository.All().FirstOrDefaultAsync();
            var recipe = await recipeRepository.All().FirstOrDefaultAsync();
            var totalProductCount = report.CountOfKg + report.DefectiveKg;
            commonAccount.TotalCash -= report.TotalProfit;
            commonAccount.Eggs += totalProductCount * recipe.Eggs;
            commonAccount.Salt += totalProductCount * recipe.Salt;
            commonAccount.Soda += totalProductCount * recipe.Soda;
            commonAccount.Kefir += totalProductCount * recipe.Kefir;
            commonAccount.Vanila += totalProductCount * recipe.Vanila;
            commonAccount.Sugar += totalProductCount * recipe.Sugar;
            commonAccount.Oil += totalProductCount * recipe.Oil;

            reportRepository.Delete(report);
            await reportRepository.SaveChangesAsync();
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
                        Date = model.Date,
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

                    report.Date = model.Date;
                    report.CountOfKg = model.CountOfKg;
                    report.DefectiveKg = model.DefectiveKg;
                    report.TotalProfit = model.TotalProfit;
                }

                await reportRepository.SaveChangesAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception: {ex.GetType()}; Сообщение об ошибке: {ex.Message}; StackTrace: {ex.StackTrace}");
            }
           
            return result;
        }
    }
}
