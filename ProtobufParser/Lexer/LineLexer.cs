using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class LineLexer : Lexer
    {
        private const char newLine = '\n';

        private readonly List<Token> tokens;
        private int position;
        private bool reset;

        public LineLexer(string input)
        {
            this.tokens = new List<Token>();
            this.reset = true;

            var lines = input.Split(newLine);
            for (var l = 0; l < lines.Length; l++)
            {
                var token = new Token(l, TokenType.Unparsed, lines[l]);
                tokens.Add(token);
            }
        }

        public Maybe<Token> Next()
        {
            if (reset && tokens.Count > 0)
            {
                reset = false;
                position = 0;
                return Maybe<Token>.Some(tokens[position]);
            }
            else if (position + 1 < tokens.Count)
            {
                position++;
                return Maybe<Token>.Some(tokens[position]);
            }

            return Maybe<Token>.None();
        }

        public void Reset()
        {
            reset = true;
        }
    }
}
