using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HuffmanCodingCompression.BitIO;
using HuffmanCodingCompression.DataStructures;

namespace HuffmanCodingCompression
{
    public static class HuffmanAlgorithm
    {
        public static void EncodeString(string s, string outputFile)
        {
            // Count characters
            var countedNodes = s.GroupBy(c => c)
                .Select(g => new TreeNode
                {
                    Frequency = g.Count(),
                    Value = g.Key
                });
            
            // Prepare heap
            var nodeHeap = new MinHeap<TreeNode>(
                Comparer<TreeNode>.Create(
                    (x, y) => x.Frequency < y.Frequency ? -1 : x.Frequency == y.Frequency ? 0 : 1
                )
            );

            foreach (var node in countedNodes)
            {
                nodeHeap.Add(node);
            }
            
            
            // Generate Tree...
            while (nodeHeap.Count > 1)
            {
                var left = nodeHeap.ExtractDominating();
                var right = nodeHeap.ExtractDominating();

                var top = new TreeNode
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };
                nodeHeap.Add(top);
            }

            var finalTree = nodeHeap.GetMin();
            //
            var dict = GetTreeCodes(finalTree);
            // Encode
            using var writeStream = new FileStream(outputFile, FileMode.Create);
            var writer = new BitWriter(writeStream);
            finalTree.EncodeTree(writer);

            var treeCodes = GetTreeCodes(finalTree);
            var codesReversed = treeCodes
                .ToDictionary(x => x.Value, x => x.Key);
            foreach (var c in s)
            {
                foreach (var bit in codesReversed[c])
                {
                    writer.WriteBit((byte) (bit - '0'));
                }
            }
            writer.Flush(); 
            writeStream.Close();
        }

        public static Dictionary<string, char> GetTreeCodes(TreeNode root)
        {
            // Find codes decoding the tree
            Dictionary<string, char> codeRepr = new();
            Dfs(root, codeRepr);
            return codeRepr;
        }
        
        public static void DecodeFile(string inputFile, string output)
        {
            using FileStream file = new FileStream(inputFile, FileMode.Open);
            using FileStream outFile = new FileStream(output, FileMode.Create);
            var reader = new BitReader(file);
            
            
            var tree = TreeDecoder.DecodeTree(reader);
            
            // Find codes decoding the tree
            var codeRepr = GetTreeCodes(tree); 
           
            var cur = "";
            while (reader.CanKeepReading())
            {
                cur += reader.ReadBit().ToString();
                if (codeRepr.TryGetValue(cur, out var value))
                {
                    outFile.WriteByte((byte) value);
                    cur = "";
                }
            }
        }

        private static void Dfs(TreeNode node, IDictionary<string, char> codeRpr, string cur = "")
        {
            if (node is null)
            {
                return;
            }
            if (node.IsLeafNode)
            {
                codeRpr[cur] = node.Value;
                return;
            }
            
            
            Dfs(node.Left, codeRpr, cur + "0");
            Dfs(node.Right, codeRpr, cur + "1");
        }
    }
}