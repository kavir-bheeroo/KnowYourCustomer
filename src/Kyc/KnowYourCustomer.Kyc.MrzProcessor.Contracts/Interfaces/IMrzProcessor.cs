using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces
{
    public interface IMrzProcessor
    {
        void ProcessMrzFile(MrzProcessRequest request);
        OcrTask GetTaskStatus(string id);
    }
}