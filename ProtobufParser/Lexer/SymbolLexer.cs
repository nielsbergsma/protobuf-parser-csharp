using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class SymbolLexer : Lexer
    {
        #region Symbols
        private readonly Dictionary<string, TokenType>[] symbolGroups = new[] {
            new Dictionary<string, TokenType>
            {
                { "=", TokenType.Assign },
                { ";", TokenType.Semicolon },
                { "{", TokenType.LeftCurlyBrace },
                { "}", TokenType.RightCurlyBrace },
                { "<", TokenType.LeftAngleBracket },
                { ">", TokenType.RightAngleBracket },
                { "[", TokenType.LeftSquareBracket },
                { "]", TokenType.RighSquaretBracket },
            }
        };
        #endregion

        private readonly Lexer source;
        private readonly Stack<Token> head;

        public SymbolLexer(Lexer source)
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

                var content = token.Value.Content;
                var matched = false;
                foreach (var symbols in symbolGroups)
                {
                    var length = (symbols.First().Key.Length);

                    for (var start = 0; !matched && start + length <= content.Length; start++)
                    {
                        var slice = content.Substring(start, length);
                        var type = default(TokenType);
                        if (!symbols.TryGetValue(slice, out type))
                        {
                            continue;
                        }

                        var end = start + length;
                        if (end < content.Length)
                        {
                            head.Push(new Token(token.Value.Line, TokenType.Unparsed, content.Substring(end)));
                        }

                        head.Push(new Token(token.Value.Line, type, slice));

                        if (start > 0)
                        {
                            head.Push(new Token(token.Value.Line, TokenType.Unparsed, content.Substring(0, start)));
                        }

                        matched = true;
                    }
                }

                if (!matched)
                {
                    return token;
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
