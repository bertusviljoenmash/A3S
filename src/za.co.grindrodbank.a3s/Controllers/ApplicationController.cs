/**
 * *************************************************
 * Copyright (c) 2019, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using za.co.grindrodbank.a3s.AbstractApiControllers;
using AutoMapper;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Repositories;
using za.co.grindrodbank.a3s.Helpers;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Controllers
{
    public class ApplicationController : ApplicationApiController
    {
        private readonly IApplicationService applicationService;
        private readonly IMapper mapper;
        private readonly IPaginationHelper paginationHelper;
        private readonly IOrderByHelper orderByHelper;

        public ApplicationController(IApplicationService applicationService, IMapper mapper, IPaginationHelper paginationHelper, IOrderByHelper orderByHelper)
        {
            this.applicationService = applicationService;
            this.mapper = mapper;
            this.paginationHelper = paginationHelper;
            this.orderByHelper = orderByHelper;
        }

        [Authorize(Policy = "permission:a3s.applications.read")]
        public async override Task<IActionResult> ListApplicationsAsync([FromQuery] int page, [FromQuery, Range(1, 1000)] int size, [FromQuery, StringLength(255, MinimumLength = 0)] string filterName, [FromQuery][StringLength(255, MinimumLength = 0)]string filterFunctionName, [FromQuery] string orderBy)
        {
            List<KeyValuePair<string, string>> orderByKeyValueList = orderByHelper.ConvertCommaSeparateOrderByStringToKeyValuePairList(orderBy);
            // Validate only correct order by components were supplied.
            orderByHelper.ValidateOrderByListOnlyContainsCertainElements(orderByKeyValueList, new List<string> { "name" });
            PaginatedResult<ApplicationModel> paginatedResult = await applicationService.GetListAsync(page, size, filterName, filterFunctionName, orderByKeyValueList);
            // Generate a K-V pair of all the current applied filters sent to the controller so that pagination header URLs can include them.
            List<KeyValuePair<string, string>> currrentFilters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("filterName", filterName),
                new KeyValuePair<string, string>("filterFunctionName", filterName),
            };

            paginationHelper.AddPaginationHeaderMetaDataToResponse(paginatedResult, currrentFilters, orderBy, "ListApplications", Url, Response);

            return Ok(mapper.Map<List<Application>>(paginatedResult.Results));
        }
    }
}
