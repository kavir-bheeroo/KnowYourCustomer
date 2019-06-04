using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Verifier.Contracts.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trulioo.Client.V1;
using Trulioo.Client.V1.Exceptions;
using Trulioo.Client.V1.Model;

namespace KnowYourCustomer.Kyc.Verifier.Trulioo.Verifiers
{
    public class TruliooSdkVerifier : IVerifier
    {
        public async Task<bool> VerifyAsync(IdentityVerificationRequest request)
        {
            var truliooClient = new TruliooApiClient("raiuniverse", "test1234!");

            var countryList = await truliooClient.Configuration.GetCountryCodesAsync("Identity Verification");

            // todo: Map country iso 3 to country iso 2 code. Abbyy returns iso 3 and Trulioo needs iso 2.
            var countryCode = countryList.FirstOrDefault(c => c.Equals(request.UserInfo.Nationality));

            if (countryCode == null)
            {
                throw new Exception($"Provided country code {countryCode} is invalid.");
            }

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

            var verifyResult = await truliooClient.Verification.VerifyAsync(verifyRequest);
            return (verifyResult.Errors == null || !verifyResult.Errors.Any()) && verifyResult.Record.RecordStatus.Equals("match");
        }
    }
}