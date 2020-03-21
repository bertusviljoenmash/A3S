/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Repositories
{
    public interface ITermsOfServiceRepository : ITransactableRepository, IPaginatedRepository<TermsOfServiceModel>
    {
        Task<TermsOfServiceModel> GetByIdAsync(Guid termsOfServiceId, bool includeRelations, bool includeFileContents);
        Task<TermsOfServiceModel> GetByAgreementNameAsync(string agreementName, bool includeRelations, bool includeFileContents);
        Task<TermsOfServiceModel> CreateAsync(TermsOfServiceModel termsOfService, bool autoAssignToPreviouslyLinkedTeams);
        Task<TermsOfServiceModel> UpdateAsync(TermsOfServiceModel termsOfService);
        Task DeleteAsync(TermsOfServiceModel termsOfService);
        Task<List<TermsOfServiceModel>> GetListAsync();
        Task<string> GetLastestVersionByAgreementName(string agreementName);
        Task<List<Guid>> GetAllOutstandingAgreementsByUserAsync(Guid userId);
        Task<PaginatedResult<TermsOfServiceModel>> GetPaginatedListAsync(int page, int pageSize, bool includeRelations, string filterAgreementName, List<KeyValuePair<string, string>> orderBy);
    }
}
