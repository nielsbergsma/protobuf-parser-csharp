using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Enumeration
    {
        private readonly string name;
        private readonly EnumerationValue[] values;
        private readonly Option[] options;

        public Enumeration(string name, IEnumerable<EnumerationValue> values, IEnumerable<Option> options)
        {
            this.name = name;
            this.values = values.ToArray();
            this.options = options.ToArray();
        }

        public string Name
        {
            get { return name; }
        }
        
        public IEnumerable<EnumerationValue> Values
        {
            get { return values; }
        }

        public IEnumerable<Option> Options
        {
            get { return options; }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Enumeration;

            return other != null
                && other.name == name;
        }
    }    
}
