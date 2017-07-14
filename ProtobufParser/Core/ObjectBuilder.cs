using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public interface ObjectBuilder
    {
        void StartComposed(string name, string type);
        void EndComposed();

        void Field(string name, double value);
        void Field(string name, float value);
        void Field(string name, int value);
        void Field(string name, bool value);
        void Field(string name, string value);
        void Field(string name, byte[] value);
    }
}