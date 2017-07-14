using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Encoding
{
    public enum FieldType
    {
        Varint = 0,
        Number64bit,
        LengthDelimited,
        GroupStart,
        GroupEnd,
        Number32bit,
    }
}