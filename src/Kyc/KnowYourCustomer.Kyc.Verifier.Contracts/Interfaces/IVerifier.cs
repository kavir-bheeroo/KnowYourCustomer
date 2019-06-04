using KnowYourCustomer.Kyc.Verifier.Contracts.Models;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces
{
    public interface IVerifier
    {
        Task<bool> VerifyAsync(IdentityVerificationRequest request);
    }
}