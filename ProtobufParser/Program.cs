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
    class Program
    {
        static void Main(string[] args)
        {
            var source = File.ReadAllText(@"c:\\temp\\test.proto");
            var tokens = ProtobufLexer.Lex(source);

            for (var t = 0; t < tokens.Length; t++)
            {
                Console.WriteLine($"Token [{t.ToString("000")}][{tokens.At(t).Type}] {tokens.At(t).Content}");
            }

            var definition = Parser.Parser.ParseDefinition(tokens);
        }
    }
}
