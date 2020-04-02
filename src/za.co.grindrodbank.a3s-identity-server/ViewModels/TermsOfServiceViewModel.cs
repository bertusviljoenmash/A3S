/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
namespace za.co.grindrodbank.a3sidentityserver.ViewModels
{
    public class TermsOfServiceViewModel : TermsOfServiceInputModel
    {
        public string HtmlContents { get; set; }
        public string CssContents { get; set; }
        public int AgreementCount { get; set; }
        public string AgreementName { get; set; }
    }
}
