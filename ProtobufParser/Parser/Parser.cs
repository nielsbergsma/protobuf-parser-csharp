using ProtobufParser.Lexer;
using ProtobufParser.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Parser
{
    public static class Parser
    {
        public static Schema ParseSchema(TokenStream stream)
        {
            var definition = new Schema();
            for (var position = 0; position < stream.Length; )
            {
                position = ParseLine(stream, position, definition);
            }
            return definition;
        }

        public static int ParseLine(TokenStream stream, int position, Schema definition)
        {
            //syntax
            if (stream.IsAt(position, TokenType.Syntax, TokenType.Assign, TokenType.String))
            {
                position = ParseSyntax(stream, position, definition);
            }
            //imports
            else if (stream.IsAt(position, TokenType.Import))
            {
                position = ParseImport(stream, position, definition);
            }
            //package
            else if (stream.IsAt(position, TokenType.Package, TokenType.Identifier))
            {
                position = ParsePackage(stream, position, definition);
            }
            //option
            else if (stream.IsAt(position, TokenType.Option, TokenType.Identifier, TokenType.Assign))
            {
                position = ParseOption(stream, position, definition);
            }
            //enum
            else if (stream.IsAt(position, TokenType.Enum, TokenType.Identifier, TokenType.LeftCurlyBrace))
            {
                position = ParseEnumeration(stream, position, definition, string.Empty);
            }
            //message
            else if (stream.IsAt(position, TokenType.Message, TokenType.Identifier, TokenType.LeftCurlyBrace))
            {
                position = ParseMessage(stream, position, definition, string.Empty);
            }
            else
            {
                position = stream.SkipToNextExpression(position);
            }
            
            return position;
        }

        public static int ParseSyntax(TokenStream stream, int position, Schema definition)
        {
            var version = stream.At(position + 2).Content;
            if (version != "proto3")
            {
                throw new NotSupportedException("Only proto3 syntax is supported");
            }

            return stream.SkipToNextExpression(position);
        }

        public static int ParseImport(TokenStream stream, int position, Schema definition)
        {
            var @public = stream.IsAt(position + 1, TokenType.Public);
            var path = stream.At(position + 1).Content;

            if (@public)
            {
                path = stream.At(position + 2).Content;
            }

            var import = new Import(path, @public);
            definition.Add(import);

            return stream.SkipToNextExpression(position);
        }

        public static int ParsePackage(TokenStream stream, int position, Schema definition)
        {
            var name = stream.At(position + 1).Content;
            var package = new Package(name);
            definition.Set(package);

            return stream.SkipToNextExpression(position);
        }

        public static int ParseOption(TokenStream stream, int position, Schema definition)
        {
            var name = stream.At(position + 1).Content;
            var value = stream.At(position + 3).Content;
            var option = new Option(name, value);
            definition.Set(option);

            return stream.SkipToNextExpression(position);
        }

        public static int ParseMessage(TokenStream stream, int position, Schema definition, string parent)
        {
            var name = parent + stream.At(position + 1).Content;
            var fields = new List<Field>();

            for (position += 3; position < stream.Length && !stream.IsAt(position, TokenType.RightCurlyBrace);)
            { 
                //field
                if (stream.IsAt(position, TokenType.Identifier, TokenType.Identifier, TokenType.Assign, TokenType.Number) 
                    || stream.IsAt(position, TokenType.Repeated, TokenType.Identifier, TokenType.Identifier, TokenType.Assign, TokenType.Number)
                    || stream.IsAt(position, TokenType.Map, TokenType.LeftAngleBracket, TokenType.Identifier, TokenType.Comma, TokenType.Identifier, TokenType.RightAngleBracket, TokenType.Identifier, TokenType.Assign, TokenType.Number))
                {
                    var field = default(Field);
                    position = ParseField(stream, position, out field);
                    fields.Add(field);
                }
                //enum
                else if (stream.IsAt(position, TokenType.Enum, TokenType.Identifier, TokenType.LeftCurlyBrace))
                {
                    position = ParseEnumeration(stream, position, definition, name + ".");
                }
                //message
                else if (stream.IsAt(position, TokenType.Message, TokenType.Identifier, TokenType.LeftCurlyBrace))
                {
                    position = ParseMessage(stream, position, definition, name + ".");
                }
                //oneof
                else if (stream.IsAt(position, TokenType.OneOf, TokenType.Identifier, TokenType.LeftCurlyBrace))
                {
                    for(position += 3; position < stream.Length && !stream.IsAt(position, TokenType.RightCurlyBrace);)
                    {
                        var field = default(Field);
                        position = ParseField(stream, position, out field);
                        fields.Add(field);
                    }
                    position++;
                }
            }

            var message = new Message(name, fields);
            definition.Add(message);

            return position + 1;
        }
        
        public static int ParseEnumeration(TokenStream stream, int position, Schema definition, string parent)
        {
            var name = parent + stream.At(position + 1).Content;
            var options = new List<Option>();
            var values = new List<EnumerationValue>();
            
            for (position += 3;
                position < stream.Length && !stream.IsAt(position, TokenType.RightCurlyBrace);
                position = stream.SkipToNextExpression(position))
            {
                if (stream.IsAt(position, TokenType.Option, TokenType.Identifier, TokenType.Assign) && position + 4 < stream.Length)
                {
                    var optionName = stream.At(position + 1).Content;
                    var optionValue = stream.At(position + 3).Content;
                    var option = new Option(optionName, optionValue);
                    options.Add(option);
                }
                else if (stream.IsAt(position, TokenType.Identifier, TokenType.Assign, TokenType.Number))
                {
                    var enumerationName = stream.At(position).Content;
                    var enumerationValue = int.Parse(stream.At(position + 2).Content);
                    var enumerationItem = new EnumerationValue(enumerationName, enumerationValue);
                    values.Add(enumerationItem);
                }
            }

            var enumeration = new Enumeration(name, values, options);
            definition.Add(enumeration);

            return position + 1;
        }

        private static int ParseField(TokenStream stream, int position, out Field field)
        {
            var repeated = stream.IsAt(position, TokenType.Repeated);
            if (repeated)
            {
                position++;
            }

            var type = stream.At(position).Content;
            if (stream.IsAt(position, TokenType.Map))
            {
                var mapKey = stream.At(position + 2).Content;
                var mapValue = stream.At(position + 4).Content;
                type = $"{mapKey}:{mapValue}";
                position += 5;
            }

            var name = stream.At(position + 1).Content;
            var tag = int.Parse(stream.At(position + 3).Content);
            var options = new List<Option>();

            //option
            if (stream.IsAt(position + 4, TokenType.LeftSquareBracket, TokenType.Identifier, TokenType.Assign))
            {
                var optionName = stream.At(position + 5).Content;
                var optionValue = stream.At(position + 7).Content;
                var option = new Option(optionName, optionValue);
                options.Add(option);
            }

            field = new Field(name, type, tag, repeated, options);

            return stream.SkipToNextExpression(position);
        }
    }
}
