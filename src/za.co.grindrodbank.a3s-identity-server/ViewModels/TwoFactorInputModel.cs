/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System.ComponentModel.DataAnnotations;

namespace za.co.grindrodbank.a3sidentityserver.ViewModels
{
    public class TwoFactorInputModel
    {
        [Required]
        public string OTP { get; set; }

        public bool IsRecoveryCode { get; set; }
        public string RedirectUrl { get; set; }
        public string Username { get; set; }
    }
}
