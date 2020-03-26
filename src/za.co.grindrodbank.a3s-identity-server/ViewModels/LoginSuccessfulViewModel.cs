/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
namespace za.co.grindrodbank.a3sidentityserver.ViewModels
{
    public class LoginSuccessfulViewModel
    {
        public string RedirectUrl { get; set; }
        public string TwoFAUrl { get; set; }
        public bool Show2FARegMessage { get; set; } = false;
        public bool TwoFAAlreadyEnabled { get; set; }
        public string UserId { get; set; }
    }
}
