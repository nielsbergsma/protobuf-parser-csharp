using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public interface Visitor
    {
        void Visit(Definition definition);
        void Visit(Package package);
        void Visit(Import import);
        void Visit(Option option);
        void Visit(Enumeration enumeration);
        void Visit(EnumerationValue enumerationValue);
        void Visit(Message message);
        void Visit(Field field);
    }
}
