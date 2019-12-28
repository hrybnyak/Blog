using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class InvalidPasswordException: Exception
    {
        public override string Message { get; }
        public InvalidPasswordException() : base()
        {
            Message = "User tried to enter email that was already taken";
        }
        public InvalidPasswordException(string str) : base(str) { }
        public InvalidPasswordException(string str, Exception inner) : base(str, inner)
        { }
        protected InvalidPasswordException(System.Runtime.Serialization.SerializationInfo si,
        System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
        public override string ToString() { return Message; }
    }
}
