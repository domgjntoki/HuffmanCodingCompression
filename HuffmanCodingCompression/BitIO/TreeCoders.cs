using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HuffmanCodingCompression.DataStructures;

namespace HuffmanCodingCompression.BitIO
{
    public static class TreeEncoder
    {
        public static void EncodeTree(this TreeNode node, Stream s)
        {
            var bitWriter = new BitWriter(s);
            node.EncodeTree(bitWriter);

        }
        public static void EncodeTree(this TreeNode node, BitWriter bw)
        {
            if (node is not null && node.IsLeafNode)
            {
                bw.WriteBit(1);
                bw.WriteByte((byte) node.Value);
            }
            else
            {
                bw.WriteBit(0);
                node?.Left.EncodeTree(bw);
                node?.Right.EncodeTree(bw);
            }
        }
    }

    public static class TreeDecoder
    {
        public static TreeNode DecodeTree(BitReader br)
        {
            if (br.ReadBit() == 1)
            {
                var charByte = br.ReadByte();
                return new TreeNode((char) charByte);
            }
            
            var leftChild = DecodeTree(br);
            var rightChild = DecodeTree(br);
            return new TreeNode('0', leftChild, rightChild);
        }

        
        public static TreeNode DecodeTree(Stream file)
        {
            var bitReader = new BitReader(file);
            return DecodeTree(bitReader);
        }
    }
}