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
 * The version of the OpenAPI document: 1.0.5
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
    public abstract class FunctionApiController : ControllerBase
    { 
        /// <summary>
        /// Create a new function.
        /// </summary>
        /// <remarks>Create a new function.</remarks>
        /// <param name="functionSubmit"></param>
        /// <response code="200">OK.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to create functions.</response>
        /// <response code="404">Function related entity (such as permissions) not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPost]
        [Route("/functions")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(Function))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> CreateFunctionAsync([FromBody]FunctionSubmit functionSubmit);

        /// <summary>
        /// Delete a function.
        /// </summary>
        /// <remarks>Deletes a function from A3S.</remarks>
        /// <param name="functionId">The UUID of the function to delete.</param>
        /// <response code="204">No Content.</response>
        /// <response code="400">Invalid parameters.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - Not authorized to delete functions.</response>
        /// <response code="404">Function not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpDelete]
        [Route("/functions/{functionId}")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> DeleteFunctionAsync([FromRoute][Required]Guid functionId);

        /// <summary>
        /// Get a function.
        /// </summary>
        /// <remarks>Get a function by its UUID.</remarks>
        /// <param name="functionId">function</param>
        /// <response code="200">OK.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to access the function.</response>
        /// <response code="404">Function not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [Route("/functions/{functionId}")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(Function))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> GetFunctionAsync([FromRoute][Required]Guid functionId);

        /// <summary>
        /// Search for functions.
        /// </summary>
        /// <remarks>Search for functions.</remarks>
        /// <param name="permissions">If this field is set, then the permission list is filled in </param>
        /// <param name="page">The page to view.</param>
        /// <param name="size">The size of a page.</param>
        /// <param name="filterDescription">A search query filter on the description</param>
        /// <param name="orderBy">a comma separated list of fields in their sort order. Ascending order is assumed. Append desc after a field to indicate descending order.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to access the list of functions.</response>
        /// <response code="404">Function list not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [Route("/functions")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Function>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> ListFunctionsAsync([FromQuery]bool permissions, [FromQuery]int page, [FromQuery][Range(1, 20)]int size, [FromQuery][StringLength(255, MinimumLength=0)]string filterDescription, [FromQuery]List<string> orderBy);

        /// <summary>
        /// Update a function.
        /// </summary>
        /// <remarks>Update a function by its UUID.</remarks>
        /// <param name="functionId">The UUID of the function.</param>
        /// <param name="functionSubmit"></param>
        /// <response code="200">OK</response>
        /// <response code="204">No Content.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to update the function.</response>
        /// <response code="404">Functions not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPut]
        [Route("/functions/{functionId}")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(Function))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> UpdateFunctionAsync([FromRoute][Required]Guid functionId, [FromBody]FunctionSubmit functionSubmit);
    }
}
