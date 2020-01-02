using System;

namespace BLL.Exceptions
{
    public class NotEnoughtRightsException : Exception
    {
        public override string Message { get; }
        public NotEnoughtRightsException() : base()
        {
            Message = "This user doens't have enouth rigths to perform this action";
        }
        public NotEnoughtRightsException(string str) : base(str) { }
        public NotEnoughtRightsException(string str, Exception inner) : base(str, inner)
        { }
        protected NotEnoughtRightsException(System.Runtime.Serialization.SerializationInfo si,
        System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }
        public override string ToString() { return Message; }
    }
}
