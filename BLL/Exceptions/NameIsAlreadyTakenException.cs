using System;

namespace BLL.Exceptions
{
   
        public class NameIsAlreadyTakenException : Exception
        {
            public override string Message { get; }
            public NameIsAlreadyTakenException() : base()
            {
                Message = "User tried to enter name that was already taken";
            }
            public NameIsAlreadyTakenException(string str) : base(str) { }
            public NameIsAlreadyTakenException(string str, Exception inner) : base(str, inner)
            { }
            protected NameIsAlreadyTakenException(System.Runtime.Serialization.SerializationInfo si,
            System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
            public override string ToString() { return Message; }
        }
    
}
