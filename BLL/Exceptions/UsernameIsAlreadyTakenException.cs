using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
   
        public class UsernameIsAlreadyTakenException : Exception
        {
            public override string Message { get; }
            public UsernameIsAlreadyTakenException() : base()
            {
                Message = "User tried to enter username that was already taken";
            }
            public UsernameIsAlreadyTakenException(string str) : base(str) { }
            public UsernameIsAlreadyTakenException(string str, Exception inner) : base(str, inner)
            { }
            protected UsernameIsAlreadyTakenException(System.Runtime.Serialization.SerializationInfo si,
            System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
            public override string ToString() { return Message; }
        }
    
}
