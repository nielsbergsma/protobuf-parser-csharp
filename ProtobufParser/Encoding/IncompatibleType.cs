using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Encoding
{
    public class IncompatibleType : Exception
    {
        public IncompatibleType(string message) 
            : base(message)
        {
        }
    }
}
