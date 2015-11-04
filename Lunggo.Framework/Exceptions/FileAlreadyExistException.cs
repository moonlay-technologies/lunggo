using System;

namespace Lunggo.Framework.Exceptions
{
    public class FileAlreadyExistException : Exception
    {
        public FileAlreadyExistException() : base() { 
        }
        public FileAlreadyExistException(string message) : base(message) { }
        public FileAlreadyExistException(string message, System.Exception inner) : base(message, inner) { }

        protected FileAlreadyExistException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
