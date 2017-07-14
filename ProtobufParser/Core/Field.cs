using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Field
    {
        private readonly string name;
        private readonly string type;
        private readonly int tag;
        private readonly bool repeated;
        private readonly List<Option> options;

        public Field(string name, string type, int tag, bool repeated, IEnumerable<Option> options)
        {
            this.name = name;
            this.type = type;
            this.tag = tag;
            this.repeated = repeated;
            this.options = options.ToList();
        }

        public string Name
        {
            get { return name; }
        }

        public string Type
        {
            get { return type; }
        }

        public int Tag
        {
            get { return tag; }
        }

        public bool Repeated
        {
            get { return repeated; }
        }

        public IEnumerable<Option> Options
        {
            get { return options; }
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }

        public void Decode(Schema definition, Stream stream, ObjectBuilder decoder)
        {

        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Field;

            return other != null
                && other.name == name;
        }
    }
}
