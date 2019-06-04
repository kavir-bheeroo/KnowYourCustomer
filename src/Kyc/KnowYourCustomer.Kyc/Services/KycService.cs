using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;

namespace KnowYourCustomer.Kyc.Services
{
    public class KycService : IKycService
    {
        private readonly IMrzProcessor _mrzProcessor;

        public KycService(IMrzProcessor mrzProcessor)
        {
            _mrzProcessor = Guard.IsNotNull(mrzProcessor, nameof(mrzProcessor));
        }

        public void ProcessPassport(KycFile model)
        {
            var mrzRequest = new MrzProcessRequest { FileName = model.FileName, FilePath = model.FilePath };
            var mrzResponse = _mrzProcessor.ProcessMrzFile(mrzRequest);
        }
    }
}