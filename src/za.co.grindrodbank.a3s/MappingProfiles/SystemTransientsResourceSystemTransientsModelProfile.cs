/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
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
                                                                        .ForMember(dest => dest.CapturerUuid, opt => opt.MapFrom(src => src.RequesterGuid))
                                                                        .ForMember(dest => dest.CapturerName, opt => opt.MapFrom(src => src.RequesterName))
                                                                        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                                                                        .ForMember(dest => dest.CapturedDate, opt => opt.MapFrom(src => src.RequestedDate))
                                                                        .ForMember(dest => dest.LatestTransientRoleFunctions, opt => opt.MapFrom(src => src.LatestActiveRoleFunctionTransients))
                                                                        .ForMember(dest => dest.LatestTransientRoleChildRoles, opt => opt.MapFrom(src => src.LatestActiveChildRoleTransients))
                                                                        .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.RoleId));
        }
    }
}
