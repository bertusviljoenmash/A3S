/**
 * *************************************************
 * Copyright (c) 2019, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using NLog;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Exceptions;
using za.co.grindrodbank.a3s.Helpers;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3s.Repositories;

namespace za.co.grindrodbank.a3s.Services
{
    public class TermsOfServiceService : ITermsOfServiceService
    {
        private readonly ITermsOfServiceRepository termsOfServiceRepository;
        private readonly IMapper mapper;
        private readonly ICompressionHelper compressionHelper;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public TermsOfServiceService(ITermsOfServiceRepository termsOfServiceRepository, ICompressionHelper compressionHelper, IMapper mapper)
        {
            this.termsOfServiceRepository = termsOfServiceRepository;
            this.compressionHelper = compressionHelper;
            this.mapper = mapper;
        }

        public async Task<TermsOfService> CreateAsync(TermsOfServiceSubmit termsOfServiceSubmit, Guid createdById)
        {
            // Start transactions to allow complete rollback in case of an error
            BeginAllTransactions();

            try
            {
                // This will only map the first level of members onto the model. User IDs and Policy IDs will not be.
                var termsOfServiceModel = mapper.Map<TermsOfServiceModel>(termsOfServiceSubmit);
                termsOfServiceModel.ChangedBy = createdById;
                termsOfServiceModel.AgreementName = termsOfServiceModel.AgreementName.Trim();
                termsOfServiceModel.Version = await GetNewAgreementVersion(termsOfServiceModel.AgreementName);

                ValidateFileCompatibility(termsOfServiceModel.AgreementFile);

                // TODO: Implement mass teams update if AutoUpdate == true

                var createdTermsOfService = mapper.Map<TermsOfService>(await termsOfServiceRepository.CreateAsync(termsOfServiceModel));

                // All successful
                CommitAllTransactions();

                return createdTermsOfService;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                RollbackAllTransactions();
                throw;
            }
        }

        public async Task DeleteAsync(Guid termsOfServiceId)
        {
            var termsOfService = await termsOfServiceRepository.GetByIdAsync(termsOfServiceId, false);

            if (termsOfService == null)
                throw new ItemNotFoundException($"Terms of Service entry with GUID '{termsOfServiceId}' not found.");

            // TODO: Implement user acceptance check before delete

            await termsOfServiceRepository.DeleteAsync(termsOfService);
        }

        public async Task<TermsOfService> GetByIdAsync(Guid termsOfServiceId, bool includeRelations = false)
        {
            return mapper.Map<TermsOfService>(await termsOfServiceRepository.GetByIdAsync(termsOfServiceId, includeRelations));
        }

        public async Task<List<TermsOfService>> GetListAsync()
        {
            return mapper.Map<List<TermsOfService>>(await termsOfServiceRepository.GetListAsync());
        }

        private void ValidateFileCompatibility(byte[] fileContents)
        {

            try
            {
                string tempFolder = $"{ Path.GetTempPath()}{Path.DirectorySeparatorChar}{Guid.NewGuid()}";
                string filePath = $"{tempFolder}{Path.DirectorySeparatorChar}terms_of_service.tar.gz";
                string extractedFolder = $"{tempFolder}{Path.DirectorySeparatorChar}terms_of_service";
                Directory.CreateDirectory(tempFolder);

                File.WriteAllBytes(filePath, fileContents);
                compressionHelper.ExtractTarGz(filePath, extractedFolder);

                if (!File.Exists($"{extractedFolder}{Path.DirectorySeparatorChar}terms_of_service.html"))
                    throw new ItemNotProcessableException("Agreement file archive does not contain a 'terms_of_service.html' file.");

                if (!File.Exists($"{extractedFolder}{Path.DirectorySeparatorChar}terms_of_service.css"))
                    throw new ItemNotProcessableException("Agreement file archive does not contain a 'terms_of_service.css' file.");

                // Cleanup
                Directory.Delete(tempFolder, true);
            }
            catch (IOException ex)
            {
                logger.Error(ex);
                throw new ItemNotProcessableException("An IO error occurred during the validation of the agreement file.");
            }
        }

        private async Task<string> GetNewAgreementVersion(string agreementName)
        {
            string latestVersion = await termsOfServiceRepository.GetLastestVersionByAgreementName(agreementName);
            string newVersion = string.Empty;

            if (latestVersion == null)
            {
                newVersion = $"{DateTime.Now.Year}.1";
            }
            else
            {
                var splitVersion = latestVersion.Split('.');

                if (splitVersion.Length != 2 || !int.TryParse(splitVersion[0], out int outTest) || !int.TryParse(splitVersion[1], out outTest) || int.Parse(splitVersion[0]) < DateTime.Now.Year)
                    newVersion = $"{DateTime.Now.Year}.1";
                else
                    newVersion = $"{splitVersion[0]}.{(int.Parse(splitVersion[1]) + 1)}";
            }

            return newVersion;
        }

        private void BeginAllTransactions()
        {
            termsOfServiceRepository.InitSharedTransaction();
        }

        private void CommitAllTransactions()
        {
            termsOfServiceRepository.CommitTransaction();
        }

        private void RollbackAllTransactions()
        {
            termsOfServiceRepository.RollbackTransaction();
        }

    }
}
