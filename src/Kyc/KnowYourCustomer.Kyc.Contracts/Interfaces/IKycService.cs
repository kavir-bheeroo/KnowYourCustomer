using KnowYourCustomer.Kyc.Contracts.Models;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Contracts.Interfaces
{
    public interface IKycService
    {
        Task ProcessPassport(KycFile model);
    }
}