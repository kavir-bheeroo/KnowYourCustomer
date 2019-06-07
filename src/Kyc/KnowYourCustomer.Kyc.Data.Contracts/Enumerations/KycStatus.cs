namespace KnowYourCustomer.Kyc.Data.Contracts.Enumerations
{
    public enum KycStatus
    {
        Initiated = 0,
        Queued = 1,
        MrzFailed = 2,
        MrzPassed = 3,
        VerificationFailed = 4,
        VerificationPassed = 5
    }
}