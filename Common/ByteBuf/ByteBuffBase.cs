using System.Collections.Generic;
using System.Text;

namespace Basement.Common.ByteBuf
{
    public abstract class ByteBuffBase
    {
        protected int offset;
        protected static readonly UTF8Encoding utf8 = new UTF8Encoding();
        protected byte[] bytes;
        protected List<byte> bytesDynamic;

        protected ByteBuffBase(byte[] bytes)
        {
            this.bytes = bytes;
        }

        protected ByteBuffBase(List<byte> bytesDynamic)
        {
            this.bytesDynamic = bytesDynamic;
        }
        
        public void SetOffset(int offset)
        {
            this.offset = offset;
        }

        public int Length()
        {
            if (bytes != null)
                return bytes.Length;

            return bytesDynamic.Count;
        }
        
        public byte[] GetBuffer()
        {
            if (bytes != null)
                return bytes;
            
            return bytesDynamic.ToArray();
        }

    }
}
