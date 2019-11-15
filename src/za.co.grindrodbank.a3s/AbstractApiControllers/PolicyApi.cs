/**
 * *************************************************
 * Copyright (c) 2019, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
/*
 * A3S
 *
 * API Definition for the A3S. This service allows authentication, authorisation and accounting.
 *
 * The version of the OpenAPI document: 1.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using za.co.grindrodbank.a3s.Attributes;
using Microsoft.AspNetCore.Authorization;
using za.co.grindrodbank.a3s.A3SApiResources;

namespace za.co.grindrodbank.a3s.AbstractApiControllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class PolicyApiController : ControllerBase
    { 
        /// <summary>
        /// Get a policy.
        /// </summary>
        /// <remarks>Get a policy by its UUID.</remarks>
        /// <param name="policyId">policy</param>
        /// <response code="200">OK.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to access the policy.</response>
        /// <response code="404">Policy not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [Route("/policies/{policyId}")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(Policy))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> GetPolicyAsync([FromRoute][Required]Guid policyId);

        /// <summary>
        /// Search for policies
        /// </summary>
        /// <remarks>Search for policies</remarks>
        /// <param name="size">The size of a page.</param>
        /// <param name="filterDescription">A search query filter on the description</param>
        /// <param name="orderBy">a comma separated list of fields in their sort order. Ascending order is assumed. Append desc after a field to indicate descending order.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to access the list of policies.</response>
        /// <response code="404">Policy list not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [Route("/policies")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Team>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> ListPoliciesAsync([FromQuery][Range(1, 20)]int size, [FromQuery][StringLength(255, MinimumLength=0)]string filterDescription, [FromQuery]List<string> orderBy);

        /// <summary>
        /// Update a policy.
        /// </summary>
        /// <remarks>Update a policy by its UUID.</remarks>
        /// <param name="policyId">The UUID of the policy.</param>
        /// <param name="policySubmit"></param>
        /// <response code="200">OK.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to update the policy.</response>
        /// <response code="404">Policy not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPut]
        [Route("/policies/{policyId}")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(Policy))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> UpdatePolicyAsync([FromRoute][Required]Guid policyId, [FromBody]PolicySubmit policySubmit);
    }
}
