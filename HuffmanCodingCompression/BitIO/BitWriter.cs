using System.IO;

namespace HuffmanCodingCompression.BitIO
{
    public class BitWriter  
    {
        private readonly bool[] _curByte = new bool[8];
        private byte _curBitIndex = 0;
        private readonly BinaryWriter _binaryWriter;
        
        public BitWriter(Stream output)
        {
            _binaryWriter = new BinaryWriter(output);
        }
        
        /**
         * Flushes the bitwriter, writing if there is values to
         * write still
         */
        public void Flush()
        {
            if (_curBitIndex != 0)
            {
                WriteByte(CurByte);
            }
            _binaryWriter.Flush();
        }
        
        public void WriteBit(byte value)
        {
            _curByte[_curBitIndex++] = value != 0;
            if (_curBitIndex == 8)
            {
                _binaryWriter.Write(CurByte);
                _curBitIndex = 0;
            }
        }

        public void WriteByte(byte value)
        {
            for (int i = 7; i >= 0; i--)
            {
                byte nthBit = (byte) ((value >> i) & 1);
                WriteBit(nthBit);
            }
        }

        private byte CurByte
        {
            get
            {
                byte b = 0;
                for (var i = 0; i < 8; i++)
                {
                    if (_curByte[i])
                    {
                        b |= (byte) (1 << i); 
                    }
                }
                return b;
            }
        }
    }
}