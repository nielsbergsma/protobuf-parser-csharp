using ProtobufParser.Encoding;
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
            //var data1 = new byte[] { 0xAC, 0x02 };
            //var result1 = default(int);
            //Reader.ReadVarint(data1, 0, out result1);

            //var position = 0;
            //var data = new byte[] { 0x08, 0x96, 0x01 };

            //var type = default(FieldType);
            //var field = default(int);
            //position += Reader.ReadHeader(data, 0, out type, out field);


            //var result = default(int);
            //var read = Reader.ReadVarint(data, position, out result);

            var source = File.ReadAllText(@"c:\\temp\\messages.proto");
            var tokens = ProtobufLexer.Lex(source);
            var definition = Parser.Parser.ParseSchema(tokens);

            for (var t = 0; t < tokens.Length; t++)
            {
                Console.WriteLine($"Token [{t.ToString("000")}][{tokens.At(t).Type}] {tokens.At(t).Content}");
            }

            var generator = new ElmTypeGenerator();
            var code = generator.Run(definition, "Details");
        }
    }
}
