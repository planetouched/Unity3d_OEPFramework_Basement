using System;
using System.Collections.Generic;
using System.Linq;

namespace Basement.Common.ByteBuf
{
    public class ByteBufWriter : ByteBuffBase
    {
        public ByteBufWriter(List<byte> bytes)
            : base(bytes)
        {
        }

        void Setter(byte[] value, bool reverse = true)
        {
            int len = value.Length;

            int add = offset + len - bytesDynamic.Count;
            if (add > 0)
                bytesDynamic.AddRange(Enumerable.Repeat<byte>(0, add));

            if (reverse)
                for (int i = 0; i < len; i++)
                    bytesDynamic[offset + i] = value[len - 1 - i];
            else
                for (int i = 0; i < len; i++)
                    bytesDynamic[offset + i] = value[i];

            offset += value.Length;
        }
        
        public void WriteByte(byte value)
        {
            bytesDynamic[offset] = value;
            offset += 1;
        }
        
        public void WriteShort(short value)
        {
            Setter(BitConverter.GetBytes(value));
        }
        
        public void WriteUShort(ushort value)
        {
            Setter(BitConverter.GetBytes(value));
        }
        
        public void WriteInt(int value)
        {
            Setter(BitConverter.GetBytes(value));
        }
        
        public void WriteUInt(uint value)
        {
            Setter(BitConverter.GetBytes(value));
        }
        
        public void WriteFloat(float value)
        {
            Setter(BitConverter.GetBytes(value));
        }

        public static int GetStringSizeInBytes(string str)
        {
            return utf8.GetByteCount(str);
        }

        public void WriteString(string str)
        {
            var data = utf8.GetBytes(str);
            var size = (short)bytesDynamic.Count;
            Setter(BitConverter.GetBytes(size));
            Setter(data, false);
        }

    }
}
