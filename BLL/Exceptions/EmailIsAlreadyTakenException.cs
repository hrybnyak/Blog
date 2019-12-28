using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class EmailIsAlreadyTakenException : Exception
    {
        public override string Message { get; }
        public EmailIsAlreadyTakenException() : base()
        {
            Message = "User tried to enter email that was already taken";
        }
        public EmailIsAlreadyTakenException(string str) : base(str) { }
        public EmailIsAlreadyTakenException(string str, Exception inner) : base(str, inner)
        { }
        protected EmailIsAlreadyTakenException(System.Runtime.Serialization.SerializationInfo si,
        System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
        public override string ToString() { return Message; }
    }
}
