/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Models;
using AutoMapper;

namespace za.co.grindrodbank.a3s.MappingProfiles
{
    public class RoleTransientResourceRoleTransientModelProfile : Profile
    {
        public RoleTransientResourceRoleTransientModelProfile()
        {
            CreateMap<RoleTransientModel, RoleTransient>().ForMember(dest => dest.RState, opt => opt.MapFrom(src => src.R_State))
                                                          .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                                                          .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                                          .ForMember(dest => dest.ApprovalCount, opt => opt.MapFrom(src => src.ApprovalCount))
                                                          .ForMember(dest => dest.RequiredApprovalCount, opt => opt.MapFrom(src => src.RequiredApprovalCount));
            CreateMap<RoleTransient, RoleTransientModel>().ForMember(dest => dest.R_State, opt => opt.MapFrom(src => src.RState));
        }
    }
}
