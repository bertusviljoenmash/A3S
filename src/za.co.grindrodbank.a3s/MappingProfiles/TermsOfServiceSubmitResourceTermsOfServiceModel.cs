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
    public class TermsOfServiceSubmitResourceTermsOfServiceModel : Profile
    {
        public TermsOfServiceSubmitResourceTermsOfServiceModel()
        {
            CreateMap<TermsOfServiceSubmit, TermsOfServiceModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid))
                                        .ForMember(dest => dest.AgreementFile, opt => opt.MapFrom(src => Convert.FromBase64String(src.AgreementFileData)));
            CreateMap<TermsOfServiceModel, TermsOfServiceSubmit>().ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.Id))
                                              .ForMember(dest => dest.AgreementFileData, opt => opt.MapFrom(src => Convert.ToBase64String(src.AgreementFile)));
        }
    }
}
