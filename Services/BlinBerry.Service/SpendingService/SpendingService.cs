﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Data.Repositories;
using BlinBerry.Services.Common.SpendingService;
using BlinBerry.Services.Common.SpendingService.Models;
using GlobalContants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlinBerry.Service.SpendingService
{
    public class SpendingService : ISpendingService
    {
        private readonly IRepository<Spending> spendingRepository;

        private readonly IRepository<State> accountRepository;

        private readonly IMapper mapper;

        private readonly ILogger<SpendingService> logger;

        public SpendingService(IRepository<Spending> spendingRepository, ILogger<SpendingService> logger, IRepository<State> accountRepository, IMapper mapper)
        {
            this.spendingRepository = spendingRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public IQueryable<SpendingDto> GetList()
        {
            return mapper.ProjectTo<SpendingDto>(spendingRepository.AllAsNoTracking().OrderByDescending(x => x.Date));
        }

        public async Task RemoveAsync(Guid id)
        {
            var spending = await spendingRepository.GetByIdAsync(id);
            var commonAccount = await accountRepository.All().FirstOrDefaultAsync();
            commonAccount.TotalCash += spending.Money;
            commonAccount.Eggs += spending.Eggs;
            commonAccount.Salt += spending.Salt;
            commonAccount.Soda += spending.Soda;
            commonAccount.Kefir += spending.Kefir;
            commonAccount.Vanila += spending.Vanila;
            commonAccount.Sugar += spending.Sugar;
            commonAccount.Oil += spending.Oil;

            spendingRepository.Delete(spending);
            await spendingRepository.SaveChangesAsync();
        }

        public async Task<OperationResult> SaveAsync(SpendingDto model, Guid userId)
        {
            var result = new OperationResult();
            var account = await accountRepository.All().OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            //Last info about account 
            try
            {
                var spending = await spendingRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

                if (spending == null)
                {
                   
                    var newSpec = mapper.Map<Spending>(model);

                    account.TotalCash -= model.Money;
                    account.Kefir -= model.Kefir;
                    account.Oil -= model.Oil;
                    account.Salt -= model.Salt;
                    account.Eggs -= model.Eggs;
                    account.Vanila -= model.Vanila;
                    account.Sugar -= model.Sugar;
                    account.Soda -= model.Soda;
                    
                    spendingRepository.Add(newSpec);
                }
                else
                {
                    account.TotalCash -= (model.Money - spending.Money);
                    account.Kefir -= (model.Kefir  - spending.Kefir); //литр
                    account.Oil -= (model.Oil - spending.Oil); // литр
                    account.Salt -= (model.Salt - spending.Salt); // кг
                    account.Eggs -= (model.Eggs - spending.Eggs); // штук
                    account.Vanila -= (model.Vanila - spending.Vanila); // грамм
                    account.Sugar -= (model.Sugar - spending.Sugar); // кг
                    account.Soda -= (model.Soda - spending.Soda); //грамм

                    mapper.Map(model, spending);

                }
                
                await spendingRepository.SaveChangesAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception: {ex.GetType()}; Сообщение об ошибке: {ex.Message}; StackTrace: {ex.StackTrace}");
            }

            return result;
        }

        public async Task<SpendingDto> PrepareForEditView(Guid? id)
        {
            var spending = await spendingRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<SpendingDto>(spending);
        }

        public async Task<SpendingDto> DetailsAsync(Guid Id)
        {
            var procurementInfo = await spendingRepository.GetByIdAsync(Id);
            var info = mapper.Map<SpendingDto>(procurementInfo);
            return info;
        }
    }
}
