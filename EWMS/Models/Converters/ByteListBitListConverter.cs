using System.Collections;
using System.Collections.Generic;

namespace EWMS.Models.Converters
{
    internal class ByteListBitListConverter
    {
        public static List<bool> ByteListToBitList(List<byte> bytes)
        {
            List<bool> bitsList = new();

            foreach (byte b in bytes)
            {
                byte[] bytee = new byte[1];
                bytee[0] = b;
                BitArray bitArray = new(bytee);

                foreach (bool bit in bitArray)
                {
                    bitsList.Add(bit);
                }
            }

            return bitsList;
        }

        public static List<byte> BitListToByteList(List<bool> bits)
        {
            List<byte> bytesList = new();

            int counter = 0;
            bool[] tempBoolArray = new bool[8];
            foreach (bool b in bits)
            {
                tempBoolArray[counter] = b;

                if (counter == 7)
                {
                    BitArray tempBitArray = new(tempBoolArray);
                    byte[] bytee = new byte[1];
                    tempBitArray.CopyTo(bytee, 0);
                    bytesList.Add(bytee[0]);
                    counter = 0;
                }
                else
                {
                    counter++;
                }
            }

            return bytesList;
        }
    }
}
