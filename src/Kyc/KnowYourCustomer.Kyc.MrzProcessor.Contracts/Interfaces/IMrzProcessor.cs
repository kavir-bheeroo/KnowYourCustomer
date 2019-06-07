using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces
{
    public interface IMrzProcessor
    {
        MrzSubmitResponse ProcessMrzFile(MrzSubmitRequest request);
        MrzStatusResponse GetMrzTaskStatus(MrzStatusRequest request);
    }
}