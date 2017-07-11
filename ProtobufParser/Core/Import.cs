using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Import
    {
        private readonly string path;
        private readonly bool @public;

        public Import(string path, bool @public)
        {
            this.path = path;
            this.@public = @public;
        }

        public string Path
        {
            get { return path; }
        }

        public bool Public
        {
            get { return @public; }
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }

        public override int GetHashCode()
        {
            var hash = path.GetHashCode();
            hash = (hash * 397) ^ @public.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Import;

            return other != null
                && other.path == path
                && other.@public == @public;
        }
    }
}
