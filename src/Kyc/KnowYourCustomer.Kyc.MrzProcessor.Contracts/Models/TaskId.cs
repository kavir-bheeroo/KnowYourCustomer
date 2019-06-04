using System;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models
{
    public class TaskId : IEquatable<TaskId>
    {
        public TaskId(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(TaskId b)
        {
            return b.Id == Id;
        }

        public string Id { get; }
    }
}