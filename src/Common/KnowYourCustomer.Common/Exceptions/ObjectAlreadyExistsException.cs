using System;

namespace KnowYourCustomer.Common.Exceptions
{

    [Serializable]
    public class ObjectAlreadyExistsException : Exception
    {
        public ObjectAlreadyExistsException() { }
        public ObjectAlreadyExistsException(string message) : base(message) { }
        public ObjectAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected ObjectAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}