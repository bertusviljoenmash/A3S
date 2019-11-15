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
    public abstract class SecurityContractApiController : ControllerBase
    { 
        /// <summary>
        /// Idempotently applies a security contract definition.
        /// </summary>
        /// <remarks>Idempotently applies a security contract definition to the A3S instance.</remarks>
        /// <param name="securityContract"></param>
        /// <response code="200">OK.</response>
        /// <response code="204">No Content.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="422">Non-Processible Entity - The security contract was correctly structured, but there are business rule or constraint violations, preventing it from being applied.</response>
        /// <response code="403">Forbidden - Not authorized to apply Security Contracts.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpPut]
        [Route("/securityContracts")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(SecurityContract))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 422, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> ApplySecurityContractAsync([FromBody]SecurityContract securityContract);

        /// <summary>
        /// Returns the entire security contract.
        /// </summary>
        /// <remarks>Returns the entire security contract for the current state of A3S.</remarks>
        /// <response code="200">OK</response>
        /// <response code="204">No Content.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Forbidden - You are not authorized to access the security contract.</response>
        /// <response code="404">Security contract not found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [Route("/securityContracts")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(SecurityContract))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 401, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 403, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 500, type: typeof(ErrorResponse))]
        public abstract Task<IActionResult> GetSecurityContractAsync();
    }
}
