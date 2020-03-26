/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System.Collections.Generic;

namespace za.co.grindrodbank.a3sidentityserver.ViewModels
{
    public class RegisterTwoFactorAuthenticatorCompleteViewModel
    {
        public IEnumerable<string> RecoveryCodes { get; set; }
        public string RedirectUrl { get; set; }
    }
}
