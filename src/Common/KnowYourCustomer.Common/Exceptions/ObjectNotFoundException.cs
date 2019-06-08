using System;
using System.Runtime.Serialization;

namespace KnowYourCustomer.Common.Exceptions
{

    [Serializable]
    public class ObjectNotFoundException : ExceptionBase
    {
        public ObjectNotFoundException(string customMessage) : base(customMessage)
        {
        }

        public ObjectNotFoundException(
            string customMessage,
            Exception innerException) : base(customMessage, innerException)
        {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}