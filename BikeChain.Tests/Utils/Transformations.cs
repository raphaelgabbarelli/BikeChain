using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeChain.Tests.Utils
{
    public static class Transformations
    {
        /// <summary>
        /// Takes a hex representation of a byte array, and converts it to an actual byte array
        /// </summary>
        /// <param name="hex">string that represent the hexadecimal values of a byte array</param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToHexString(byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "").ToLower();
        }
    }
}
