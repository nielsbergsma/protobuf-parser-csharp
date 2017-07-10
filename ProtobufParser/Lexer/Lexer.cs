using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public interface Lexer
    {
        Maybe<Token> Next();
        void Reset();
    }
}
