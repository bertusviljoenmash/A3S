/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Exceptions;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Services
{
    public class SecurityContractService : ISecurityContractService
    {
        private readonly ISecurityContractClientService clientService;
        private readonly ISecurityContractApplicationService securityContractApplicationService;
        private readonly ISecurityContractDefaultConfigurationService securityContractDefaultConfigurationService;

        public SecurityContractService(ISecurityContractApplicationService securityContractApplicationService, ISecurityContractClientService clientService,
            ISecurityContractDefaultConfigurationService securityContractDefaultConfigurationService)
        {
            this.securityContractApplicationService = securityContractApplicationService;
            this.clientService = clientService;
            this.securityContractDefaultConfigurationService = securityContractDefaultConfigurationService;
        }

        public async Task ApplySecurityContractDefinitionAsync(SecurityContract securityContract, Guid updatedById, bool dryRun = false)
        {
            // Start transactions to allow complete rollback in case of an error
            BeginAllTransactions();

            try
            {
                SecurityContractDryRunResult securityContractDryRunResult = new SecurityContractDryRunResult();
                // First apply all of the application(micro-service) definitions that present within the Security Contract.
                // All the components of a security contract are optional, so check for this here.
                if (securityContract.Applications != null && securityContract.Applications.Count > 0)
                {
                    foreach (var applicationSecurityContractDefinition in securityContract.Applications)
                    {
                        await securityContractApplicationService.ApplyResourceServerDefinitionAsync(applicationSecurityContractDefinition, updatedById, dryRun, securityContractDryRunResult);
                    }
                }

                // Apply any clients that may be defined within the security contract.
                if (securityContract.Clients != null && securityContract.Clients.Count > 0)
                {
                    foreach (var clientSecurityContractDefinition in securityContract.Clients)
                    {
                        await clientService.ApplyClientDefinitionAsync(clientSecurityContractDefinition, dryRun, securityContractDryRunResult);
                    }
                }

                // Apply any default configurations that may be defined within the security contract.
                if (securityContract.DefaultConfigurations != null && securityContract.DefaultConfigurations.Count > 0)
                {
                    foreach (var defaultConfiguration in securityContract.DefaultConfigurations)
                    {
                        await securityContractDefaultConfigurationService.ApplyDefaultConfigurationDefinitionAsync(defaultConfiguration, updatedById, dryRun, securityContractDryRunResult);
                    }
                }

                if (!dryRun)
                {
                    // All successful
                    CommitAllTransactions();
                }
                else
                {
                    var securityContractDryRunException = new SecurityContractDryRunException
                    {
                        ValidationErrors = securityContractDryRunResult.ValidationErrors,
                        ValidationWarnings = securityContractDryRunResult.ValidationWarnings
                    };

                    throw securityContractDryRunException;
                }
            }
            catch
            {
                RollbackAllTransactions();
                throw;
            }
        }

        private void BeginAllTransactions()
        {
            securityContractApplicationService.InitSharedTransaction();
            securityContractDefaultConfigurationService.InitSharedTransaction();
            clientService.InitSharedTransaction();
        }

        private void CommitAllTransactions()
        {
            securityContractApplicationService.CommitTransaction();
            securityContractDefaultConfigurationService.CommitTransaction();
            clientService.CommitTransaction();
        }

        private void RollbackAllTransactions()
        {
            securityContractApplicationService.RollbackTransaction();
            securityContractDefaultConfigurationService.RollbackTransaction();
            clientService.RollbackTransaction();
        }

        public async Task<SecurityContract> GetSecurityContractDefinitionAsync()
        {
            var securityContract = new SecurityContract()
            {
                Applications = new List<SecurityContractApplication>(),
                Clients = new List<Oauth2ClientSubmit>(),
                DefaultConfigurations = new List<SecurityContractDefaultConfiguration>()
            };

            // Retrieve all Application definitions
            securityContract.Applications.AddRange(await securityContractApplicationService.GetResourceServerDefinitionsAsync());

            // Retrieve all Client definitions
            securityContract.Clients.AddRange(await clientService.GetClientDefinitionsAsync());

            // Retrieve all Default Configuration definitions
            securityContract.DefaultConfigurations.Add(await securityContractDefaultConfigurationService.GetDefaultConfigurationDefinitionAsync());

            return securityContract;
        }
    }
}
