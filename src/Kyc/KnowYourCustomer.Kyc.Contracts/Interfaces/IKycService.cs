using KnowYourCustomer.Kyc.Contracts.Models;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Contracts.Interfaces
{
    public interface IKycService
    {
        Task<InitiateKycResponseModel> InitiateKyc(InitiateKycRequestModel requestModel);
        Task<CheckMrzStatusResponseModel> CheckMrzTaskStatus(CheckMrzStatusRequestModel requestModel);
        Task<VerificationResponseModel> VerifyIdentity(VerificationRequestModel requestModel);
    }
}