using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class EnumerationValue
    {
        private readonly string name;
        private readonly int value;

        public EnumerationValue(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
        }

        public int Value
        {
            get { return value; }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as EnumerationValue;

            return other != null
                && other.name == name;
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
