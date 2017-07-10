using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Model
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

        public override bool Equals(object obj)
        {
            var other = obj as Option;

            return other.name == name
                && other.value == value;
        }

        public override int GetHashCode()
        {
            var hash = name.GetHashCode();
            hash = (hash * 397) ^ value.GetHashCode();
            return hash;
        }
    }
}
