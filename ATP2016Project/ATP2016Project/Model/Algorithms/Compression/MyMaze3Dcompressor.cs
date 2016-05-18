using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Compression
{
    class MyMaze3Dcompressor : ICompressor
    {
        private bool firstTimeCompress = true; //help us to determine if it is the first time we use the compressor
        private bool firstTimeDecompress = true;

        public byte[] compress(byte[] data)
        {
            //this list will contain the compress data
            List<byte> compressed = new List<byte>();
            //compress the settings of the maze(dimensions,start point,goal point)
            int i = 0;
            if (firstTimeCompress)
            {
                int numOfSettings = 9;
                for (int j = 0; j < numOfSettings; j++)
                {
                    compressed.Add(data[j]);
                }
                //compress the maze
                i = numOfSettings;
            }
            int swapDigit = 0;
            while (i < data.Length)
            {
                //take a byte from the data
                byte byteFromData = data[i];
                byte count = 0;
                while (i < data.Length && data[i] == swapDigit % 2 && count < byte.MaxValue)
                {
                    count++;
                    i++;
                }
                swapDigit++;
                compressed.Add(count);
            }
            firstTimeCompress = false;
            return compressed.ToArray();
        }

        public byte[] decompress(byte[] data)
        {
            List<byte> decompressed = new List<byte>();
            //decompress the settings of the maze(dimensions,start point,goal point)
            int i = 0;
            if (firstTimeDecompress)
            {
                int numOfSettings = 9;
                for (int j = 0; j < numOfSettings; j++)
                {
                    decompressed.Add(data[j]);
                }
                //decompress the maze
                i = numOfSettings;
            }
            byte swapDigit = 0;
            while (i < data.Length)
            {

                //
                for (int counter = data[i]; counter > 0; counter--)
                {
                    decompressed.Add(swapDigit);
                }
                i++;
                swapDigit = (byte)((swapDigit + 1) % 2);
            }
            firstTimeDecompress = false;
            return decompressed.ToArray();
        }
    }
}
