using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class StringLexer : Lexer
    {
        private const char doublequote = '\"';
        private const char backslash = '\\';

        private readonly Lexer source;
        private readonly Stack<Token> head;

        public StringLexer(Lexer source)
        {
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

                var line = token.Value.Content;
                var start = line.IndexOf(doublequote);
                if (start < 0)
                {
                    return token;
                }

                var end = start + 1;
                var ended = false;
                for (var escaped = false; !ended && end < line.Length; end++)
                {
                    if (line[end] == backslash)
                    {
                        escaped = !escaped;
                    }
                    else if (line[end] == doublequote && !escaped)
                    {
                        ended = true;
                    }
                    else
                    {
                        escaped = false;
                    }
                }

                if (!ended)
                {
                    end = line.Length;
                }

                if (end < line.Length)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Unparsed, line.Substring(end)));
                }

                if (!ended)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Undetermined, line.Substring(start)));
                }
                else
                {
                    head.Push(new Token(token.Value.Line, TokenType.String, line.Substring(start + 1, end - start - 2)));
                }

                if (start > 0)
                {
                    head.Push(new Token(token.Value.Line, TokenType.Unparsed, line.Substring(0, start)));
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
