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

        public static Package Default
        {
            get { return new Package("<empty>"); }
        }

        public Package(string name)
        {
            this.name = name;
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
