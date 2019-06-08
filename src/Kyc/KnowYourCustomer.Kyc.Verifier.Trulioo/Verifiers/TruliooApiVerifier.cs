using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Verifier.Contracts.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Trulioo.Client.V1.Model;

namespace KnowYourCustomer.Kyc.Verifier.Trulioo.Verifiers
{
    public class TruliooApiVerifier : IVerifier
    {
        private readonly HttpClient _httpClient;

        public TruliooApiVerifier(IHttpClientFactory httpClientFactory)
        {
            Guard.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            _httpClient = httpClientFactory.CreateClient("verifier");
        }

        public async Task<bool> VerifyAsync(IdentityVerificationRequest request)
        {
            var countryCode = "AU";

            // Leveraging the SDK's models instead of re-creating them from scratch.
            var verifyRequest = new VerifyRequest
            {
                AcceptTruliooTermsAndConditions = true,
                Demo = true,
                ConfigurationName = "Identity Verification",
                CountryCode = countryCode,
                DataFields = new DataFields
                {
                    PersonInfo = new PersonInfo
                    {
                        FirstGivenName = request.UserInfo.FirstName,
                        FirstSurName = request.UserInfo.LastName,
                        DayOfBirth = request.UserInfo.DateOfBirth.Day,
                        MonthOfBirth = request.UserInfo.DateOfBirth.Month,
                        YearOfBirth = request.UserInfo.DateOfBirth.Year,
                        Gender = request.UserInfo.Gender
                    },
                    Passport = new Passport
                    {
                        Mrz1 = request.PassportInfo.Mrz1,
                        Mrz2 = request.PassportInfo.Mrz2,
                        Number = request.PassportInfo.Number,
                        DayOfExpiry = request.PassportInfo.DateOfExpiry.Day,
                        MonthOfExpiry = request.PassportInfo.DateOfExpiry.Month,
                        YearOfExpiry = request.PassportInfo.DateOfExpiry.Year
                    },
                }
            };

            var payload = JsonConvert.SerializeObject(verifyRequest);
            var response = await _httpClient.PostAsync("trial/verifications/v1/verify", new StringContent(payload, Encoding.ASCII, "application/json"));

            // todo: deserialize result to check for match or no match.
            return response.IsSuccessStatusCode;
        }
    }
}