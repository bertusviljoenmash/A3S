/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
namespace za.co.grindrodbank.a3sidentityserver.ViewModels
{
    public class TwoFactorViewModel : TwoFactorInputModel
    {
        public bool AuthenticatorConfigured { get; set; }
    }
}
