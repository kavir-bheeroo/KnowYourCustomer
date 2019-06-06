namespace KnowYourCustomer.Kyc.Data.Contracts.Entities
{
    public enum KycStatus
    {
        Queued = 0,
        MrzFailed = 1,
        MrzPassed = 2,
        VerificationFailed = 3,
        VerificationPassed = 4
    }
}