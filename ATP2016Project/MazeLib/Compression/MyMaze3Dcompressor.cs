using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    /// In this class we implement our naive compressor for the representation of our maze
    /// the maze represent as a byte array in the following structure:
    /// [maze settings(dimensions,starting point, goal point), maze bytes array]
    /// the setting took 9 places so we have to start compressing from the 9th cell and on
    /// 
    /// we choose the naive compressing method as follow: 
    /// we assume that we start with 0. we write the sequence of zeros than the sequence of ones, than back we write the sequence of zeros
    /// and etc..
    /// for example: before compression we get the input: 1,1,1,0,0,1,1,1 => after compression: 0,3,2,3
    /// </summary>
    public class MyMaze3Dcompressor : ICompressor
    {
        private bool firstTimeCompress = true; //help us to determine if it is the first time we use the compressor
        private bool firstTimeDecompress = true;

        /// <summary>
        /// this function gets an array of uncompressed bytes array and compress them using the naive compression method
        /// as mention above in the description
        /// </summary>
        /// <param name="data">array of uncompressed bytes that represent the maze</param>
        /// <returns>compressed maze bytes array</returns>
        public byte[] compress(byte[] data)
        {
            //this list will contain the compress data
            List<byte> compressed = new List<byte>();
            //compress the settings of the maze(dimensions,start point,goal point)
            int i = 0;
            //check if it's the first iteration
            //if it is, we add the settings of the maze (first 9 places) normally
            if (firstTimeCompress)
            {
                i = enterMazeSettings(data, compressed);
            }
            int swapDigit = 0; //help us to track if the current sequnce is of zeros or ones
            while (i < data.Length)
            {
                //take a byte from the data
                byte byteFromData = data[i];
                byte count = 0;
                //checks if the current byte is the same like our swapDigit and updateds the counters accordingly
                while (i < data.Length && data[i] == swapDigit % 2 && count < byte.MaxValue)
                {
                    count++;
                    i++;
                }
                swapDigit++; //change the current digit of sequence (from 0 to 1 and from 1 to 0)
                compressed.Add(count);
            }
            firstTimeCompress = false; //to assure that next time we will not enter the first 9 cells of the bytes array without compressing
            return compressed.ToArray();
        }

        /// <summary>
        /// an helper method
        /// enter the settings of the maze to the compressed list
        /// </summary>
        /// <param name="data">the original bytes array of the maze</param>
        /// <param name="compressed">the empty compressed list</param>
        /// <returns>the compressed list with the 9 bytes of settings</returns>
        private static int enterMazeSettings(byte[] data, List<byte> compressed)
        {
            int i;
            int numOfSettings = 9;
            for (int j = 0; j < numOfSettings; j++)
            {
                compressed.Add(data[j]);
            }
            //update the i to be 9
            i = numOfSettings;
            return i;
        }

        /// <summary>
        /// gets a compressed bytes array 
        /// and decompress it to a list and returns it
        /// </summary>
        /// <param name="data">the compressed byte array</param>
        /// <returns>decompressed bytes array</returns>
        public byte[] decompress(byte[] data)
        {
            List<byte> decompressed = new List<byte>();
            //decompress the settings of the maze(dimensions,start point,goal point)
            int i = 0;
            //check if it's the first iteration
            //if it is, we add the settings of the maze (first 9 places) normally
            if (firstTimeDecompress)
            {
                i = enterMazeSettings(data, decompressed);
            }
            byte swapDigit = 0;
            while (i < data.Length)
            {
                fillDigitSequence(data, decompressed, i, swapDigit);
                i++;
                swapDigit = (byte)((swapDigit + 1) % 2); //updates 1 to 0 and 0 to 1
            }
            firstTimeDecompress = false;
            return decompressed.ToArray();
        }

        /// <summary>
        /// loop the number of sequence of the specific digit
        /// and add it to the decompressed list
        /// </summary>
        /// <param name="data">the compressed bytes array</param>
        /// <param name="decompressed">list of decompressed bytes</param>
        /// <param name="i">the location in the compressed bytes array</param>
        /// <param name="swapDigit">the desired digit to add to the decompressed list</param>
        private static void fillDigitSequence(byte[] data, List<byte> decompressed, int i, byte swapDigit)
        {
            for (int counter = data[i]; counter > 0; counter--)
            {
                decompressed.Add(swapDigit);
            }
        }
    }
}
