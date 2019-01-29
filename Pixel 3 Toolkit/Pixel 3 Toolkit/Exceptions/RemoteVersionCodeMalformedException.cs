using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_3_Toolkit.Exceptions
{
    public class RemoteVersionCodeMalformedException : Exception
    {
        public RemoteVersionCodeMalformedException()
        {
        }

        public RemoteVersionCodeMalformedException(string message) : base(message)
        {
        }

        public RemoteVersionCodeMalformedException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
