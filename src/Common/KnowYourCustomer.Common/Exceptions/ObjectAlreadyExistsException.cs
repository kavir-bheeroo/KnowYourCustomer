using System;
using System.Runtime.Serialization;

namespace KnowYourCustomer.Common.Exceptions
{

    [Serializable]
    public class ObjectAlreadyExistsException : ExceptionBase
    {
        public ObjectAlreadyExistsException(string customMessage) : base(customMessage)
        {
        }

        /// <inheritdoc />
        protected ObjectAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}