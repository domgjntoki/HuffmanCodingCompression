using System.IO;

namespace HuffmanCodingCompression.BitIO
{
    public class BitReader
    {
        private readonly bool[] _curByte = new bool[8];
        private byte _curBitIndex;
        private readonly BinaryReader _binaryReader;

        public BitReader(Stream s)
        {
            _binaryReader = new BinaryReader(s);
            
            CopyToCurByte(_binaryReader.ReadByte());
        }

        public bool CanKeepReading() 
        {
            return _binaryReader.BaseStream.Position != _binaryReader.BaseStream.Length;
        }
        
        public byte ReadBit()
        {
            if (_curBitIndex == 8)
            {
                CopyToCurByte(_binaryReader.ReadByte());
                _curBitIndex = 0;
            }

            return  (byte) (_curByte[_curBitIndex++] ? 1 : 0);
        }

        public byte ReadByte()
        {
            byte b = 0;
            for (var i = 7; i >= 0; i--)
            {
                if (ReadBit() == 1)
                {
                    b |= (byte) (1 << i);
                }
            }
            return b; 
        }

        private void CopyToCurByte(byte b)
        {
            for (var i = 0; i < 8; i++)
            { 
                _curByte[i] = ((b >> i) & 1) != 0; 
            }
        }
    }
}