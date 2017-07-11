using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public interface RuntimeDecoder
    {
        void StartComposedValue(string name, string type);
        void EndComposedValue();

        void DoubleValue(string name, double value);
        void FloatValue(string name, float value);
        void IntegerValue(string name, int value);
        void BooleanValue(string name, bool value);
        void StringValue(string name, string value);
        void BytesValue(string name, byte[] value);
    }
}
