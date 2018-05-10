using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BikeChain
{
    public class BlockRepository
    {
        /// <summary>
        /// Generates the genesys block for BikeChain
        /// </summary>
        /// <returns></returns>
        public Block CreateGenesisBlock()
        {
            using (var hasher = SHA256.Create())
            {
                return new Block(new DateTime(1998, 6, 7), hasher.ComputeHash(Encoding.UTF8.GetBytes("keepcalmandstravaon")), hasher.ComputeHash(Encoding.UTF8.GetBytes("thepirate")), new byte[] { });
            }
        }
    }
}
