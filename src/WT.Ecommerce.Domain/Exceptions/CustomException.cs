using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace WT.Ecommerce.Domain.Exceptions
{
    [Serializable]
    public class CustomException : Exception
    {
        public string Detail { get; private set; }

        public CustomException() { }

        public CustomException(string message, string detail = null) : base(message)
        {
            Detail = detail ?? message;
        }

        public CustomException(string message, string detail, Exception inner) : base(message, inner)
        {
            Detail = detail ?? message;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("Detail", Detail);
            base.GetObjectData(info, context);
        }
    }
}
