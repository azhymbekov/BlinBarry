﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Services.Common.CommonInfoService.Models;
using BlinBerry.Services.Common.ProcurementService.Models;
using BlinBerry.Services.Common.SpendingService.Models;

namespace BlinBerry.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CommonInfoDto, CommonMoneyAndProducts>();
            CreateMap<CommonMoneyAndProducts, CommonInfoDto>();

            CreateMap<ProcurementDto, ProductProcurement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ProductProcurement, ProcurementDto>();

            CreateMap<Spending, SpendingDto>();
            CreateMap<SpendingDto, Spending>();


            CreateMap<ProcurementDto, CommonMoneyAndProducts>();
            CreateMap<CommonMoneyAndProducts, ProcurementDto>();
        }
    }
}