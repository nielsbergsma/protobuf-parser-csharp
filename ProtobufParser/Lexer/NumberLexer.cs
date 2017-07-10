using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class NumberLexer : Lexer
    {
        private readonly Lexer source;
        private readonly Stack<Token> head;
        private readonly Regex pattern;

        public NumberLexer(Lexer source)
        {
            this.source = source;
            this.head = new Stack<Token>();
            this.pattern = new Regex(@"[0-9]+", RegexOptions.IgnoreCase);
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

                var number = default(Match);
                foreach (Match match in pattern.Matches(content))
                {
                    if (!IsSurroundedByIdentifierChar(content, match.Index, match.Index + match.Length))
                    {
                        number = match;
                        break;
                    }
                }

                if (number == null)
                {
                    return token;
                }

                var start = number.Index;
                var end = start + number.Length;
                if (end < content.Length)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Unparsed, content.Substring(end)));
                }

                head.Push(new Token(token.Value.Line, TokenType.Number, number.Value));

                if (start > 0)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Unparsed, content.Substring(0, start)));
                }
            }
        }

        private bool IsSurroundedByIdentifierChar(string content, int start, int end)
        {
            if (start > 0 && IsIdentifierChar(content[start - 1]))
            {
                return true;
            }
            else if (end < content.Length && IsIdentifierChar(content[end]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsIdentifierChar(char value)
        {
            return char.IsLetterOrDigit(value) || value == '_';
        }

        public void Reset()
        {
            head.Clear();
            source.Reset();
        }
    }
}
