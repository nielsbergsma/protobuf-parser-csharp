using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Package
    {
        private readonly string name;

        public Package(string name)
        {
            this.name = name;
        }

        public static Package Undefined
        {
            get { return new Package("<undefined>"); }
        }

        public string Name
        {
            get { return name; }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Package;

            return other != null
                && other.name == name;
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
