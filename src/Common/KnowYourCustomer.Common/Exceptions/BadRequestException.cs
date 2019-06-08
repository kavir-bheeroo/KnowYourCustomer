using System;
using System.Runtime.Serialization;

namespace KnowYourCustomer.Common.Exceptions
{
    [Serializable]
    public class BadRequestException : ExceptionBase
    {
        public BadRequestException(
            string customMessage) : base(customMessage)
        {
        }

        public BadRequestException(
            string customMessage,
            Exception innerException) : base(customMessage, innerException)
        {
        }

        /// <inheritdoc />
        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}