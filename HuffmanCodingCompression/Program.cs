using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HuffmanCodingCompression.BitIO;

namespace HuffmanCodingCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = File.ReadAllText(@"F:\Desktop\Programacao\C#\Lore ipsum.txt");
            const string filepath = @"F:\Desktop\Programacao\C#\encoded.huff";

            var watch = Stopwatch.StartNew();
            HuffmanAlgorithm.EncodeString(s, filepath);
            watch.Stop();
            Console.WriteLine($"Time to Encode: {watch.ElapsedMilliseconds}");
            
            watch = Stopwatch.StartNew();
            HuffmanAlgorithm.DecodeFile(filepath, @"F:\Desktop\Programacao\C#\Decoded.txt");
            watch.Stop();
            Console.WriteLine($"Time to Decode: {watch.ElapsedMilliseconds}"); 
            
        }
    }
}