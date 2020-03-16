using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Data.Repositories;
using BlinBerry.Services.Common.ProcurementService;
using BlinBerry.Services.Common.ProcurementService.Models;
using GlobalContants;
using Microsoft.EntityFrameworkCore;

namespace BlinBerry.Service.ProcurementService
{
    public class ProcurementService : IProcurementService
    {
        private readonly IRepository<ProductProcurement> procurementRepository;

        private readonly IRepository<CommonMoneyAndProducts> accountRepository;

        private readonly IMapper mapper;

        public ProcurementService(IRepository<ProductProcurement> procurementRepository, IRepository<CommonMoneyAndProducts> accountRepository, IMapper mapper)
        {
            this.procurementRepository = procurementRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        public Task<ProcurementDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProcurementDto> GetList()
        {
            var products = procurementRepository.All();
            return products.Select(x => mapper.Map<ProcurementDto>(x));
        }

        public async Task RemoveAsync(Guid id)
        {
            var procurement = await procurementRepository.GetByIdAsync(id);
            var accountInfo = await accountRepository.GetByAsync(x => x.Id == procurement.BlinBerryId);
            procurementRepository.Delete(procurement);
            accountRepository.Delete(accountInfo);
            await procurementRepository.SaveChangesAsync();
        }

        public async Task<OperationResult> SaveAsync(ProcurementDto model, Guid userId)
        {
            var result = new OperationResult();
            var procurement = await procurementRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);
            

            try
            {
                if (procurement == null)
                {
                    var commonAccount = await accountRepository.All().OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
                    var newProc = mapper.Map<ProductProcurement>(model);
                    var newTransaction = new CommonMoneyAndProducts()
                    {
                        Id = Guid.NewGuid(),
                        TotalCash = commonAccount.TotalCash - model.TotalSum,
                        Kefir = commonAccount.Kefir + model.Kefir,
                        Oil = commonAccount.Oil + model.Oil,
                        Salt = commonAccount.Salt+model.Salt,
                        Eggs = commonAccount.Eggs+model.Eggs,
                        Vanila = commonAccount.Vanila + model.Vanila,
                        Sugar = commonAccount.Sugar+model.Sugar,
                        Soda = commonAccount.Soda+model.Soda
                    };

                    newProc.BlinBerryId = newTransaction.Id;
                    accountRepository.Add(newTransaction);
                    procurementRepository.Add(newProc);
                }
                else
                {
                    var account = await accountRepository.All().FirstOrDefaultAsync(x => x.Id == procurement.BlinBerryId);

                    var previousAccount = accountRepository.All().OrderByDescending(x => x.CreatedOn).Skip(1).First();

                    account.TotalCash = previousAccount.TotalCash -  model.TotalSum;

                    account.Kefir = previousAccount.Kefir + model.Kefir; //литр
                    account.Oil = previousAccount.Oil + model.Oil; // литр
                    account.Salt = previousAccount.Salt + model.Salt; // кг
                    account.Eggs = previousAccount.Eggs + model.Eggs; // штук
                    account.Vanila = previousAccount.Vanila + model.Vanila; // грамм
                    account.Sugar = previousAccount.Sugar + model.Sugar; // кг
                    account.Soda = previousAccount.Soda + model.Soda; //грамм

                    mapper.Map(model, procurement);

                }
                await procurementRepository.SaveChangesAsync(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return result;
        }

        public async Task<ProcurementDto> PrepareForEditView(Guid? id)
        {
            var proc = await procurementRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<ProcurementDto>(proc);

        }

        public async Task<ProcurementDto> DetailsAsync(Guid procId)
        {
            var procurementInfo = await procurementRepository.GetByIdAsync(procId);
            var info = mapper.Map<ProcurementDto>(procurementInfo);
            return info;
        }
    }
}
