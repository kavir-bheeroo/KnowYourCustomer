namespace KnowYourCustomer.Kyc.Verifier.Contracts.Options
{
    public class VerificationProviderOptions
    {
        public const string DefaultSectionName = "VerificationProvider";

        /// <summary>
        /// Use api or sdk
        /// </summary>
        public string ProviderType { get; set; } = "api";
        public string Url { get; set; }
        public string HeaderKey { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}