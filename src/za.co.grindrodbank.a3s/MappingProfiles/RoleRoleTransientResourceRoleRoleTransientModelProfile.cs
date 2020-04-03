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
    public class RoleRoleTransientResourceRoleRoleTransientModelProfile : Profile
    {
        public RoleRoleTransientResourceRoleRoleTransientModelProfile()
        {
            CreateMap<RoleRoleTransientModel, RoleChildRoleTransient>().ForMember(dest => dest.RState, opt => opt.MapFrom(src => src.R_State))
                                                          .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.ParentRoleId))
                                                          .ForMember(dest => dest.ChildRoleId, opt => opt.MapFrom(src => src.ChildRoleId))
                                                          .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                                          .ForMember(dest => dest.ApprovalCount, opt => opt.MapFrom(src => src.ApprovalCount));

            CreateMap<RoleRoleTransientDetailModel, RoleChildRoleDetailedTransient>().ForMember(dest => dest.RState, opt => opt.MapFrom(src => src.R_State))
                                                          .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                                                          .ForMember(dest => dest.ChildRole, opt => opt.MapFrom(src => src.ChildRole))
                                                          .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                                          .ForMember(dest => dest.ApprovalCount, opt => opt.MapFrom(src => src.ApprovalCount));

            CreateMap<RoleModel, RoleSimple>().ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<RoleChildRoleTransient, RoleRoleTransientModel>().ForMember(dest => dest.R_State, opt => opt.MapFrom(src => src.RState));
        }
    }
}
