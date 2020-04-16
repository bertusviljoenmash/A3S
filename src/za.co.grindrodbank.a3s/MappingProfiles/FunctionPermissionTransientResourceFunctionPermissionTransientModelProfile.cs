/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using AutoMapper;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.MappingProfiles
{
    public class FunctionPermissionTransientResourceFunctionPermissionTransientModelProfile : Profile
    {
        public FunctionPermissionTransientResourceFunctionPermissionTransientModelProfile()
        {
            CreateMap<FunctionPermissionTransient, FunctionPermissionTransientModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid));
            CreateMap<FunctionPermissionTransientModel, FunctionPermissionTransient>().ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.FunctionId, opt => opt.MapFrom(src => src.FunctionId))
                                                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.PermissionId));
        }
    }
}
