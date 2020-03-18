/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using za.co.grindrodbank.a3s.Models;
using AutoMapper;
using za.co.grindrodbank.a3s.A3SApiResources;

namespace za.co.grindrodbank.a3s.MappingProfiles
{
    public class PermissionResourcePermisionModelProfile : Profile
    {
        public PermissionResourcePermisionModelProfile()
        {
            CreateMap<Permission, PermissionModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid));
            CreateMap<PermissionModel, Permission>().ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id));
        }
    }
}
