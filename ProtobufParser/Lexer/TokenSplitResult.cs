using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public class TokenSplitResult
    {
        private readonly Maybe<Token> before;
        private readonly Maybe<Token> after;
        private readonly string seperator;

        public TokenSplitResult(Maybe<Token> before, Maybe<Token> after, string seperator)
        {
            this.before = before;
            this.after = after;
            this.seperator = seperator;
        }

        public Maybe<Token> Before
        {
            get { return before; }
        }

        public Maybe<Token> After
        {
            get { return after; }
        }

        public string Seperator
        {
            get { return seperator; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as TokenSplitResult;

            return other != null
                && other.before.Equals(before)
                && other.after.Equals(after)
                && other.seperator == seperator;
        }

        public override int GetHashCode()
        {
            var hash = before.GetHashCode();
            hash = (hash * 397) ^ after.GetHashCode();
            hash = (hash * 397) ^ seperator.GetHashCode();
            return hash;
        }
    }
}
