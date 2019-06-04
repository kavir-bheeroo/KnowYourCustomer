namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models
{
    public enum TaskStatus
    {
        Unknown,
        Submitted,
        Queued,
        InProgress,
        Completed,
        ProcessingFailed,
        Deleted,
        NotEnoughCredits
    }
}