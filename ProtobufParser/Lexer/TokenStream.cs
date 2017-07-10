using ProtobufParser.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class TokenStream
    {
        private readonly Token[] tokens;

        public TokenStream(Lexer lexer)
        {
            var tokens = new List<Token>();
            for (var token = lexer.Next(); token.HasValue; token = lexer.Next())
            {
                tokens.Add(token.Value);
            }
            this.tokens = tokens.ToArray();
        }

        public TokenStream(Token[] tokens)
        {
            this.tokens = tokens;
        }

        public int Length
        {
            get { return tokens.Length; }
        }

        public bool IsAtEndOfStream(int position)
        {
            return position + 1 >= tokens.Length;
        }

        public bool IsAt(int position, TokenType type)
        {
            return position < tokens.Length && tokens[position].Type == type;
        }

        public bool IsAt(int position, TokenType type, params TokenType[] rest)
        {
            if (position >= tokens.Length || tokens[position].Type != type)
            {
                return false;
            }

            for (var r = 0; r < rest.Length; r++)
            {
                if (position + 1 + r >= tokens.Length || tokens[position + 1 + r].Type != rest[r])
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsAt(int position, string content)
        {
            return position < tokens.Length && tokens[position].Content == content;
        }

        public bool IsAnyAt(int position, params TokenType[] types)
        {
            if (position >= tokens.Length)
            {
                return false;
            }
            return types.Contains(tokens[position].Type);
        }

        public Token At(int position)
        {
            return tokens[position];
        }

        public int LineOf(int position)
        {
            if (position >= tokens.Length)
            {
                return -1;
            }
            else
            {
                return tokens[position].Line;
            }
        }
       
        public int SkipToNextExpression(int position)
        {
            if (position >= tokens.Length)
            {
                return tokens.Length - 1;
            }

            position++;
            while (position < tokens.Length && tokens[position].Type != TokenType.Semicolon)
            {
                position++;
            }
            position++;

            return position;
        }
    }
}
