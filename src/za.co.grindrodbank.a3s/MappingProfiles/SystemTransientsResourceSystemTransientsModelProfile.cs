/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Linq;
using AutoMapper;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.MappingProfiles
{
    public class SystemTransientsResourceSystemTransientsModelProfile : Profile
    {
        public SystemTransientsResourceSystemTransientsModelProfile()
        {
            CreateMap<SystemTransientsModel, SystemTransients>().ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.TransientRoles));
            CreateMap<SystemTransientsRoleModel, SystemTransientsRole>().ForMember(dest => dest.LatestActiveRoleTransient , opt => opt.MapFrom(src => src.LatestActiveRoleTransient))
                                                                        .ForMember(dest => dest.LatestTransientRoleFunctions, opt => opt.MapFrom(src => src.LatestActiveRoleFunctionTransients))
                                                                        .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.RoleId));
            CreateMap<RoleTransientModel, RoleTransientsItem>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<RoleFunctionTransientModel, RoleFunctionTransient>().ForMember(dest => dest.FunctionId, opt => opt.MapFrom(src => src.FunctionId));
        }
    }
}
