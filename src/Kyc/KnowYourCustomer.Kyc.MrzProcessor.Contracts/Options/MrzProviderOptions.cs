namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Options
{
    public class MrzProviderOptions
    {
        public const string DefaultSectionName = "MrzProvider";

        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Encoding { get; set; }
        public string StorageFolder { get; set; }
    }
}