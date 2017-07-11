using ProtobufParser.Lexer;
using ProtobufParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser
{
    public class UsageExample
    {
        static void Main(string[] args)
        {
            var source = File.ReadAllText(@"c:\\temp\\messages.proto");
            var tokens = ProtobufLexer.Lex(source);
            var definition = Parser.Parser.ParseDefinition(tokens);

            for (var t = 0; t < tokens.Length; t++)
            {
                Console.WriteLine($"Token [{t.ToString("000")}][{tokens.At(t).Type}] {tokens.At(t).Content}");
            }

            var generator = new ElmTypeGenerator();
            var code = generator.Run(definition, "Details");
        }
    }
}
