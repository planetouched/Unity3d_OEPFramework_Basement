using System;

namespace Basement.Common.ByteBuf
{
    public class ByteBufReader : ByteBuffBase
    {
        public ByteBufReader(byte[] bytes, int offset = 0) : base(bytes)
        {
            this.offset = offset;
        }

        public byte ReadByte()
        {
            return bytes[offset];
        }

        byte [] Reverse(int count)
        {
            var reverse = new byte[count];

            int swapSize = count / 2;
            
            if (count % 2 == 1)
                reverse[swapSize + 1] = bytes[offset + swapSize + 1];

            for (int i = 0; i < swapSize; i++)
            {
                reverse[i] = bytes[offset + count - 1 - i];
                reverse[count - 1 - i] = bytes[offset + i];
            }
            return reverse;
        }
        
        public short ReadShort()
        {
            var value = BitConverter.ToInt16(Reverse(2), 0);
            offset += 2;
            return value;
        }

        public ushort ReadUShort()
        {
            var value = BitConverter.ToUInt16(Reverse(2), 0);
            offset += 2;
            return value;
        }

        public int ReadInt()
        {
            var value = BitConverter.ToInt32(Reverse(4), 0);
            offset += 4;
            return value;
        }

        public uint ReadUInt()
        {
            var value = BitConverter.ToUInt32(Reverse(4), 0);
            offset += 4;
            return value;
        }

        public float ReadFloat()
        {
            var value = BitConverter.ToSingle(Reverse(4), 0);
            offset += 4;
            return value;
        }

        public string ReadString()
        {
            var size = ReadUShort();
            return utf8.GetString(bytes, offset += size, size);
        }
    }
}
