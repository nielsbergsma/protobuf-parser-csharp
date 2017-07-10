using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Lexer
{
    public enum TokenType
    {
        /* errors */
        Unparsed = -1,
        Undetermined = 0,

        /* comment */
        Comment,

        /* symbols */
        LeftCurlyBrace,
        RightCurlyBrace,
        LeftSquareBracket,
        RighSquaretBracket,
        LeftAngleBracket,
        RightAngleBracket,
        Semicolon,
        Assign,

        /* identifier */
        Identifier,
        
        /* literals */
        String,
        Number,
        True,
        False,

        /* keywords */
        Syntax,
        Package,
        Message,
        Option,
        Import,
        Reserved,
        Enum,
        Public,
        Repeated,
        OneOf,

        /* lexer internal */
        SingleLineCommentStart,
        StringBoundry
    }
}
