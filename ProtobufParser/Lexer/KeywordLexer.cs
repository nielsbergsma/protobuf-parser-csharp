using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class KeywordLexer : Lexer
    {
        #region Keywords
        private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "syntax", TokenType.Syntax },
            { "package", TokenType.Package },
            { "message", TokenType.Message },
            { "option", TokenType.Option },
            { "import", TokenType.Import },
            { "reserved", TokenType.Reserved },
            { "enum", TokenType.Enum },
            { "public", TokenType.Public },
            { "repeated", TokenType.Repeated },
            { "oneof", TokenType.OneOf },
            { "true", TokenType.True },
            { "false", TokenType.False },
            { "map", TokenType.Map },
        };
        #endregion

        private readonly Lexer source;

        public KeywordLexer(Lexer source)
        {
            this.source = source;
        }

        public Maybe<Token> Next()
        {
            var token = source.Next();
            if (!token.HasValue || !token.Value.Is(TokenType.Identifier))
            {
                return token;
            }

            if (!keywords.ContainsKey(token.Value.Content))
            {
                return token;
            }

            var type = keywords[token.Value.Content];
            return Maybe<Token>.Some(new Token(token.Value.Line, type, token.Value.Content));
        }

        public void Reset()
        {
            source.Reset();
        }
    }
}
