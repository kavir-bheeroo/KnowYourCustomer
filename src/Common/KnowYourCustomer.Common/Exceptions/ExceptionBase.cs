using System;
using System.Runtime.Serialization;

namespace KnowYourCustomer.Common.Exceptions
{
    [Serializable]
    public abstract class ExceptionBase : Exception
    {
        public string CustomMessage { get; }

        protected ExceptionBase(string customMessage) : base(customMessage)
        {
            CustomMessage = customMessage;
        }

        protected ExceptionBase(string customMessage, Exception innerException) : base(customMessage, innerException)
        {
            CustomMessage = customMessage;
        }

        protected ExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CustomMessage = info.GetString(nameof(CustomMessage));
        }
    }
}