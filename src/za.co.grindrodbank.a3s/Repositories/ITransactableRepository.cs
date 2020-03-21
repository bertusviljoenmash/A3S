/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
namespace za.co.grindrodbank.a3s.Repositories
{
    public interface ITransactableRepository
    {
        void InitSharedTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
