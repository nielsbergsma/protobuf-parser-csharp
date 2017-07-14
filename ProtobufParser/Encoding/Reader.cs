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
                throw new EndOfStreamException();
            }

            type = (FieldType)(header & 7);
            field = header >> 3;
            return offset + read;
        }

        public static int ReadVarint(byte[] data, int offset, out int value)
        {
            var size = 0;
            while (offset + size < data.Length && (data[offset + size] & 0x80) == 0x80 && size < 9)
            {
                size++;
            }

            value = 0;
            for (var index = size; index >= 0; index--)
            {
                value = (value << 7) | (data[offset + index]) & 0x7F;
            }

            return offset + size + 1;
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
                throw new EndOfStreamException();
            }

            value = System.Text.Encoding.UTF8.GetString(data, offset, length);
            return offset + length;
        }

        public static int ReadBytes(byte[] data, int offset, out byte[] value)
        {
            var length = default(int);
            offset += ReadVarint(data, offset, out length);
            value = new byte[length];

            var available = data.Length - offset;
            if (available < length)
            {
                throw new EndOfStreamException();
            }

            for (var b = 0; b < length; b++)
            {
                value[b] = data[offset + b];
            }

            return offset + length;
        }

        public static int ReadDouble(byte[] data, int offset, out double value)
        {
            throw new NotImplementedException();
        }

        public static int ReadFloat(byte[] data, int offset, out float value)
        {
            throw new NotImplementedException();
        }

        public static int ReadInt32(byte[] data, int offset, out int value)
        {
            throw new NotImplementedException();
        }

        public static int ReadInt64(byte[] data, int offset, out long value)
        {
            throw new NotImplementedException();
        }

        public static int ReadBool(byte[] data, int offset, out bool value)
        {
            throw new NotImplementedException();
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
}
