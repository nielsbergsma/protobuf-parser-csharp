using ProtobufParser.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Message
    {
        private readonly string name;
        private readonly List<Field> fields;

        public Message(string name, IEnumerable<Field> fields)
        {
            this.name = name;
            this.fields = fields.ToList();
        }

        public string Name
        {
            get { return name; }
        }

        public IEnumerable<Field> Fields
        {
            get { return fields; }
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }

        public void Unmarshal(Schema schema, byte[] data, int offset, ObjectBuilder builder)
        {
            for (var endOfStream = false; !endOfStream;)
            {
                var type = default(FieldType);
                var tag = default(int);
                var header = Reader.ReadHeader(data, offset, out type, out tag);
                var field = fields.FirstOrDefault(f => f.Tag == tag);

                if (header > 0 && field != null)
                {
                    if (!IsExpectedType(field, type))
                    {
                        throw new IncompatibleType($"Expected field {field.Name} to be of type {type}");
                    }

                    switch(field.Type)
                    {
                        case "string":
                            var stringValue = default(string);
                            offset += header;
                            offset += Reader.ReadString(data, offset, out stringValue);
                            builder.Field(field.Name, stringValue);
                            break;

                        case "bytes":
                            var bytesValue = default(byte[]);
                            offset += header;
                            offset += Reader.ReadBytes(data, offset, out bytesValue);
                            builder.Field(field.Name, bytesValue);
                            break;

                        case "int32":
                            var int32Value = default(int);
                            offset += header;
                            offset += Reader.ReadVarint(data, offset, out int32Value);
                            builder.Field(field.Name, int32Value);
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {
                    offset += header;
                }

                endOfStream = header == 0 || offset + 1 >= data.Length;
            }
        }

        private bool IsExpectedType(Field field, FieldType type)
        {
            switch (field.Type)
            {
                case "string":
                case "bytes":
                    return type == FieldType.LengthDelimited;

                case "int32":
                    return type == FieldType.Varint;

                case "int64":
                    return type == FieldType.Varint;

                case "varint":
                    return type == FieldType.Varint;

                default:
                    throw new NotSupportedException();
            }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Message;

            return other != null
                && other.name == name;
        }
    }
}
