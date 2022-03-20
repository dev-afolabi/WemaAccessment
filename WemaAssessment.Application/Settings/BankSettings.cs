namespace WemaAssessment.Application.Settings
{
    public class BankSettings
    {
        public const string ConfigSection = "BankSettings";
        public string BaseUrl { get; set; }
        public string Resource { get; set; }
        public string ConfigurationKey { get; set; }
    }
}
