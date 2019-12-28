using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class WrongCredentialsException : Exception
    {
        public override string Message { get; }
        public WrongCredentialsException() : base()
        {
            Message = "User entered wrong credentials";
        }
        public WrongCredentialsException(string str) : base(str) { }
        public WrongCredentialsException(string str, Exception inner) : base(str, inner)
        { }
        protected WrongCredentialsException(System.Runtime.Serialization.SerializationInfo si,
        System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
        public override string ToString() { return Message; }
    }
}
