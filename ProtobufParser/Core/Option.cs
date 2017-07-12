using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Option
    {
        private readonly string name;
        private readonly string value;

        public Option(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
        }

        public string Value
        {
            get { return value; }
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = name.GetHashCode();
                hash = (hash * 397) ^ value.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Option;

            return other.name == name
                && other.value == value;
        }
    }
}
