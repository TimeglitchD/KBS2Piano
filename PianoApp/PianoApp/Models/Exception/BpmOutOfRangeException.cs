using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PianoApp.Models.Exception
{
    class BpmOutOfRangeException : SystemException
    {
        public BpmOutOfRangeException()
        {
        }

        public BpmOutOfRangeException(string message) : base(message)
        {
            
        }

        public BpmOutOfRangeException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected BpmOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
