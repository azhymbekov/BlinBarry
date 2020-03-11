using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlinBerry.Services.Common.CommonInfoService.Models;

namespace BlinBerry.Services.Common.CommonInfoService
{
    public interface ICommonInfoAboutAccountService
    {
        double? GetAccountInfo();

        Task<CommonInfoDto> GetCommonInfo();
    }
}
