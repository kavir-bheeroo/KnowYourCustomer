using KnowYourCustomer.Kyc.Contracts.Models;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Contracts.Interfaces
{
    public interface IKycService
    {
        Task<InitiateKycResponseModel> InitiateKycAsync(InitiateKycRequestModel requestModel);
        Task<CheckMrzStatusResponseModel> CheckMrzTaskStatusAsync(CheckMrzStatusRequestModel requestModel);
        Task<VerificationResponseModel> VerifyIdentityAsync(VerificationRequestModel requestModel);
    }
}