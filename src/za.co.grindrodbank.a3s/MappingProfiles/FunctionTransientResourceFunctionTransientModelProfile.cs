/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System.Linq;
using za.co.grindrodbank.a3s.Models;
using AutoMapper;
using za.co.grindrodbank.a3s.A3SApiResources;

namespace za.co.grindrodbank.a3s.MappingProfiles
{
    public class FunctionTransientResourceFunctionTransientModelProfile : Profile
    {
        public FunctionTransientResourceFunctionTransientModelProfile()
        {
            CreateMap<FunctionTransient, FunctionTransientModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid));
            CreateMap<FunctionTransientModel, FunctionTransient>().ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.LatestTransientFunctionPermissions, opt => opt.MapFrom(src => src.LatestTransientFunctionPermissions))
                                                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId));
        }
    }
}