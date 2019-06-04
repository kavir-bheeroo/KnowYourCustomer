using KnowYourCustomer.Kyc.Contracts.Models;

namespace KnowYourCustomer.Kyc.Contracts.Interfaces
{
    public interface IKycService
    {
        void ProcessPassport(KycFile model);
    }
}