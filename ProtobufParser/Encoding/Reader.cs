using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Encoding
{
    public static class Reader
    {
        public static int ReadHeader(byte[] data, int offset, out FieldType type, out int field)
        {
            var header = default(int);
            var read = ReadVarint(data, offset, out header);
            if (read == 0)
            {
                throw new EndOfFile();
            }

            type = (FieldType)(header & 7);
            field = (int)header >> 3;
            return offset + read;
        }

        public static int ReadVarint(byte[] data, int offset, out int value)
        {
            var read = 0;
            var length = data.Length;

            while (offset + read < length && (data[offset + read] & 0x80) == 0x80 && read < 9)
            {
                read++;
            }

            value = 0;
            for (var index = read; index >= 0; index--)
            {
                value = (value << 7) | (data[offset + index]) & 0x7F;
            }

            return offset + read + 1;
        }

        public static int ReadString(byte[] data, int offset, out string value)
        {
            var length = default(int);
            offset += ReadVarint(data, offset, out length);

            if (length == 0)
            {
                value = "";
            }

            var available = data.Length - offset;
            if (available < length)
            {
                throw new EndOfFile();
            }

            value = System.Text.Encoding.UTF8.GetString(data, offset, length);
            return (int) length;
        }

        public static int SkipField(byte[] data, int offset)
        {
            var type = default(FieldType);
            var field = default(int);
            var read = ReadHeader(data, offset, out type, out field);

            switch (type)
            {
                case FieldType.Varint:
                    var value = default(int);
                    read += ReadVarint(data, offset + read, out value);
                    break;

                case FieldType.Number32bit:
                    read += 4;
                    break;

                case FieldType.Number64bit:
                    read += 8;
                    break;

                case FieldType.LengthDelimited:
                    var content = default(string);
                    read += ReadString(data, offset + read, out content);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return offset + read;
        }
    }

    public class EndOfFile : Exception
    {

    }
}
