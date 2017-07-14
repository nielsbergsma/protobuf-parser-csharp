using ProtobufParser.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser
{
    public class ElmTypeGenerator : Visitor
    {
        private readonly StringBuilder output;
        private Schema schema;
        private string messageNamePostfix;

        private int itemIndex;
        private int itemCount;
        private string enumGenerateMode;

        public ElmTypeGenerator()
        {
            output = new StringBuilder();
        }

        public string Run(Schema schema, string messageNamePostfix = "")
        {
            this.schema = schema;
            this.messageNamePostfix = CamelCase(messageNamePostfix);

            output.Clear();
            schema.Accept(this);
            return output.ToString();
        }

        public void Visit(Schema definition)
        {
            definition.Package.Accept(this);

            foreach(var import in definition.Imports)
            {
                import.Accept(this);
            }

            foreach (var enumeration in definition.Enumerations)
            {
                enumeration.Accept(this);
            }

            foreach (var message in definition.Messages)
            {
                message.Accept(this);
            }
        }

        public void Visit(Option option)
        {
            //not supported
        }

        public void Visit(Import import)
        {
            //not supported
        }

        public void Visit(Enumeration enumeration)
        {
            output.AppendLine();
            output.AppendLine();
            output.AppendLine($"value_of_{SnakeCase(enumeration.Name)} v = case v of ");
            foreach (var value in enumeration.Values)
            {
                enumGenerateMode = "ToValue";
                value.Accept(this);
            }
            output.AppendLine("  _ -> 0");

            output.AppendLine();
            output.AppendLine();
            output.AppendLine($"string_of_{SnakeCase(enumeration.Name)} v = case v of ");
            foreach(var value in enumeration.Values.GroupBy(v => v.Value))
            {
                enumGenerateMode = "ToString";
                value.First().Accept(this);
            }
            output.AppendLine("  _ -> \"?\"");
        }

        public void Visit(EnumerationValue enumerationValue)
        {
            if (enumGenerateMode == "ToValue")
            {
                output.AppendLine($"  \"{enumerationValue.Name}\" -> {enumerationValue.Value}");
            }
            else if (enumGenerateMode == "ToString")
            {
                output.AppendLine($"  {enumerationValue.Value} -> \"{enumerationValue.Name}\"");
            }
        }

        public void Visit(Message message)
        {
            output.AppendLine();
            output.AppendLine();
            output.AppendLine($"type alias {CamelCase(message.Name.Split('.').Last())}{messageNamePostfix} =");
            
            itemIndex = 0;
            itemCount = message.Fields.Count();
            if (itemCount ==  0)
            {
                output.AppendLine("   {");
                output.AppendLine("   }");
            }

            foreach(var field in message.Fields)
            {
                field.Accept(this);
            }
        }

        public void Visit(Field field)
        {
            var fieldContent = $"{Sanitize(SnakeCase(field.Name))} : {GetTypeName(field.Type, field.Repeated)}";

            if (itemIndex == 0)
            {
                output.AppendLine("   { " + fieldContent);
            }
            else
            {
                output.AppendLine("   , " + fieldContent);
            }

            itemIndex++;
            if (itemIndex == itemCount)
            {
                output.AppendLine("   }");
            }
        }

        public void Visit(Package package)
        {
            if (!package.Equals(Package.Undefined))
            {
                output.AppendLine($"module {CamelCase(package.Name)} exposing (..)");
            }
        }

        private string Sanitize(string name)
        {
            var keywords = new[] { "true", "false", "alias", "of", "case", "if", "then", "else", "type", "as", "let", "in", "module", "port", "exposing", "import", "infix", "infixl", "infixr" };
            if (keywords.Contains(name))
            {
                return name + "_";
            }
            else
            {
                return name;
            }
        }

        private string SnakeCase(string value)
        {
            var content = "";
            for (var c = 0; c < value.Length; c++)
            {
                if (c > 0 && char.IsUpper(value[c]))
                {
                    content += "_";
                }

                if (char.IsLetterOrDigit(value[c]))
                {
                    content += char.ToLower(value[c]);
                }
            }
            return content;
        }

        private string CamelCase(string value)
        {
            var content = "";
            foreach(var part in value.Split('.', '_'))
            {
                var characters = part.ToArray();
                characters[0] = char.ToUpper(characters[0]);
                content += new string(characters);
            }
            return content;
        }

        private string GetTypeName(string protobufType, bool repeated)
        {
            if (repeated)
            {
                return "List";
            }

            if (IsEnumeration(protobufType))
            {
                return "Int";
            }

            switch(protobufType)
            {
                case "double":
                case "float":
                    return "Float";

                case "int32":
                case "int64":
                case "uint32":
                case "uint64":
                case "sint32":
                case "sint64":
                case "fixed32":
                case "fixed64":
                case "sfixed32":
                case "sfixed64":
                    return "Int";

                case "bool":
                    return "Boolean";

                case "string":
                    return "String";

                case "bytes":
                    return "List";

                default:
                    return CamelCase(protobufType.Split('.').Last());
            }
        }

        private bool IsEnumeration(string protobufType)
        {
            return schema.Enumerations.Any(e => e.Name == protobufType || e.Name.EndsWith("." + protobufType));
        }
    }
}
