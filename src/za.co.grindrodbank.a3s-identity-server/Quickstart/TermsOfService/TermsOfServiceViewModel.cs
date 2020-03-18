/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
namespace za.co.grindrodbank.a3sidentityserver.Quickstart.UI
{
    public class TermsOfServiceViewModel : TermsOfServiceInputModel
    {
        public string HtmlContents { get; set; }
        public string CssContents { get; set; }
        public int AgreementCount { get; set; }
        public string AgreementName { get; set; }
    }
}
