using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Model
{
    public class Message
    {
        private readonly string name;
        private readonly List<Field> fields;

        public Message(string name, IEnumerable<Field> fields)
        {
            this.name = name;
            this.fields = fields.ToList();
        }

        public string Name
        {
            get { return name; }
        }

        public IEnumerable<Field> Fields
        {
            get { return fields; }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Message;

            return other != null
                && other.name == name;
        }
    }
}
