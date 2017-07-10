using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class SplitLexer : Lexer
    {
        private readonly string seperator;
        private readonly TokenType tokenType;
        private readonly Lexer source;
        private readonly Stack<Token> head;

        public SplitLexer(string seperator, TokenType tokenType, Lexer source)
        {
            this.seperator = seperator;
            this.tokenType = tokenType;
            this.source = source;
            this.head = new Stack<Token>();
        }

        public Maybe<Token> Next()
        {
            while (true)
            {
                var token = head.Any()
                    ? Maybe<Token>.Some(head.Pop())
                    : source.Next();

                if (!token.HasValue || !token.Value.Is(TokenType.Unparsed))
                {
                    return token;
                }

                var index = token.Value.Content.IndexOf(seperator);
                if (index < 0)
                {
                    return token;
                }

                var split = token.Value.Split(index, seperator);
                if (split.After.HasValue)
                {
                    head.Push(split.After.Value);
                }

                head.Push(new Token(token.Value.Line, tokenType, seperator));

                if (split.Before.HasValue)
                {
                    head.Push(split.Before.Value);
                }
            }
        }

        public void Reset()
        {
            head.Clear();
            source.Reset();
        }
    }
}
