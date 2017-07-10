using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class CommentIgnoreLexer : Lexer
    {
        private readonly Lexer source;

        public CommentIgnoreLexer(Lexer source)
        {
            this.source = source;
        }

        public Maybe<Token> Next()
        {
            var token = source.Next();
            while (token.HasValue && token.Value.Is(TokenType.Comment))
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
