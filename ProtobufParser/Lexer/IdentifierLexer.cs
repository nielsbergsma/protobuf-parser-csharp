using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class IdentifierLexer : Lexer
    {
        private readonly Lexer source;
        private readonly Stack<Token> head;
        private readonly Regex pattern;

        public IdentifierLexer(Lexer source)
        {
            this.source = source;
            this.head = new Stack<Token>();
            this.pattern = new Regex(@"[a-z][a-z0-9_\.]*", RegexOptions.IgnoreCase);
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

                var content = token.Value.Content;

                var identifier = default(Match);
                foreach (Match match in pattern.Matches(content))
                {
                    if (!IsSurroundedByNumber(content, match.Index, match.Index + match.Length))
                    {
                        identifier = match;
                        break;
                    }
                }

                if (identifier == null)
                {
                    return token;
                }

                var start = identifier.Index;
                var end = start + identifier.Length;
                if (end < content.Length)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Unparsed, content.Substring(end)));
                }

                head.Push(new Token(token.Value.Line, TokenType.Identifier, identifier.Value));

                if (start > 0)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Unparsed, content.Substring(0, start)));
                }
            }
        }

        private bool IsSurroundedByNumber(string content, int start, int end)
        {
            if (start > 0 && char.IsDigit(content[start - 1]))
            {
                return true;
            }
            else if (end < content.Length && char.IsDigit(content[end]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            head.Clear();
            source.Reset();
        }
    }
}
