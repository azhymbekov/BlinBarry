using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Services.Common.CommonInfoService.Models;
using BlinBerry.Services.Common.ProcurementService.Models;
using BlinBerry.Services.Common.RecipeService.Models;
using BlinBerry.Services.Common.SelesReport.Models;
using BlinBerry.Services.Common.SpendingService.Models;

namespace BlinBerry.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CommonInfoDto, State>().ReverseMap();

            CreateMap<ProcurementDto, ProductProcurement>().ReverseMap();

            CreateMap<Spending, SpendingDto>().ReverseMap();

            CreateMap<ProcurementDto, State>().ReverseMap();

            CreateMap<SeleTransaction, SeleTransactionDto>().ReverseMap();

            CreateMap<RecipeDto, Recipe>().ReverseMap();
        }
    }
}
