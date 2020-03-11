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

            var account = await accountRepository.All().FirstOrDefaultAsync();
            try
            {
                var spending = await spendingRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

                if (spending == null)
                {
                    var newSpec = mapper.Map<Spending>(model);
                    spendingRepository.Add(newSpec);
                }
                else
                {
                    mapper.Map(spending, model);
                }

                account.TotalCash -= model.Money;

                account.Kefir -= model.Kefir; //литр
                account.Oil -= model.Oil; // литр
                account.Salt -= model.Salt; // кг
                account.Eggs -= model.Eggs; // штук
                account.Vanila -= model.Vanila; // грамм
                account.Sugar -= model.Sugar; // кг
                account.Soda -= model.Soda; //грамм

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
