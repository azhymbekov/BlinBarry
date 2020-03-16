using System;
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

namespace BlinBerry.Service.SpendingService
{
    public class SpendingService : ISpendingService
    {
        private readonly IRepository<Spending> spendingRepository;

        private readonly IRepository<CommonMoneyAndProducts> accountRepository;

        private readonly IMapper mapper;

        public SpendingService(IRepository<Spending> spendingRepository, IRepository<CommonMoneyAndProducts> accountRepository, IMapper mapper)
        {
            this.spendingRepository = spendingRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }
        public IQueryable<SpendingDto> GetList()
        {
            return mapper.ProjectTo<SpendingDto>(spendingRepository.AllAsNoTracking());
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> SaveAsync(SpendingDto model, Guid userId)
        {
            var result = new OperationResult();

            //Last info about account 
            try
            {
                var spending = await spendingRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

                if (spending == null)
                {
                    var account = await accountRepository.All().OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
                    var newSpec = mapper.Map<Spending>(model);

                    var newTransaction = new CommonMoneyAndProducts()
                    {
                        Id = Guid.NewGuid(),
                        TotalCash = account.TotalCash - model.Money,
                        Kefir = account.Kefir - model.Kefir, //литр
                        Oil = account.Oil - model.Oil, // литр
                        Salt = account.Salt - model.Salt, // кг
                        Eggs = account.Eggs - model.Eggs, // штук
                        Vanila = account.Vanila - model.Vanila, // грамм
                        Sugar = account.Sugar - model.Sugar, // кг
                        Soda = account.Soda - model.Soda //грамм
                    };

                    newSpec.BlinBerryId = newTransaction.Id;
                    
                    accountRepository.Add(newTransaction);
                    
                    spendingRepository.Add(newSpec);
                }
                else
                {
                    var currentAccount = await accountRepository.GetByAsync(x => x.Id == spending.BlinBerryId);
                    var previousAccount = accountRepository.All().OrderByDescending(x => x.CreatedOn).Skip(1).First();

                    currentAccount.TotalCash = previousAccount.TotalCash - model.Money;
                    currentAccount.Kefir = previousAccount.Kefir - model.Kefir; //литр
                    currentAccount.Oil = previousAccount.Oil - model.Oil; // литр
                    currentAccount.Salt = previousAccount.Salt - model.Salt; // кг
                    currentAccount.Eggs = previousAccount.Eggs - model.Eggs; // штук
                    currentAccount.Vanila = previousAccount.Vanila - model.Vanila; // грамм
                    currentAccount.Sugar = previousAccount.Sugar - model.Sugar; // кг
                    currentAccount.Soda = previousAccount.Soda - model.Soda; //грамм

                    mapper.Map(spending, model);
                }

                await spendingRepository.SaveChangesAsync(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
