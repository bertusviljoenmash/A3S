/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.AbstractApiControllers;
using za.co.grindrodbank.a3s.Services;

namespace za.co.grindrodbank.a3s.Controllers
{
    public class TransientsController : TransientsApiController
    {
        private readonly ISystemTransientsService systemTransientsService;
        private IMapper mapper;

        public TransientsController(ISystemTransientsService systemTransientsService, IMapper mapper)
        {
            this.systemTransientsService = systemTransientsService;
            this.mapper = mapper;
        }

        public override async Task<IActionResult> GetTransientsAsync([FromQuery] int page, [FromQuery, Range(1, 1000)] int size, [FromQuery] bool includeRoles, [FromQuery] bool includeFunctions, [FromQuery] bool includeAuthModes, [FromQuery] bool includeUsers)
        {
            return Ok(mapper.Map<SystemTransients>(await systemTransientsService.GetAllSystemTransients(true)));
        }
    }
}
