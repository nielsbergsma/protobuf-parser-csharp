using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class WhitespaceIgnoreLexer : Lexer
    {
        private readonly Lexer source;

        public WhitespaceIgnoreLexer(Lexer source)
        {
            this.source = source;
        }

        public Maybe<Token> Next()
        {
            var token = source.Next();
            while (token.HasValue && token.Value.Is(TokenType.Unparsed) && string.IsNullOrWhiteSpace(token.Value.Content))
            {
                token = source.Next();
            }
            return token;
        }

        public void Reset()
        {
            source.Reset();
        }
    }
}
