using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Data.Repositories;
using BlinBerry.Services.Common.CommonInfoService;
using BlinBerry.Services.Common.CommonInfoService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlinBerry.Service.CashAndProducts
{
    public class CommonAccountService : ICommonInfoAboutAccountService
    {
        private readonly IMapper mapper;
        private readonly IRepository<CommonMoneyAndProducts> accountRepository;

        public CommonAccountService(IMapper mapper, IRepository<CommonMoneyAndProducts> accountRepository)
        {
            this.mapper = mapper;
            this.accountRepository = accountRepository;
        }
        public double? GetAccountInfo()
        {
            var currentAccount = accountRepository.AllAsNoTracking().FirstOrDefault();
            return currentAccount.TotalCash;
        }

        public async Task<CommonInfoDto> GetCommonInfo()
        {
            var info = await accountRepository.AllAsNoTracking().FirstOrDefaultAsync();
            return mapper.Map<CommonInfoDto>(info);
        }
    }
}
