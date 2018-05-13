using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

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

        /// <summary>
        /// Creates a new block
        /// </summary>
        /// <param name="previousBlock"></param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="previousBlock"/> or <paramref name="data"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="data"/> is empty (length is zero)</exception>
        /// <returns></returns>
        public Block MineBlock(Block previousBlock, byte[] data)
        {
            if (previousBlock == null) throw new ArgumentNullException("previousBlock");
            if (data == null) throw new ArgumentNullException("data", "data cannot be null");
            if (data.Length == 0) throw new ArgumentException("data cannot be empty", "data");

            DateTime timestamp = DateTime.UtcNow;
            byte[] hash = HashBlock(timestamp, previousBlock.Hash, data);

            return new Block(timestamp, previousBlock.Hash, hash, data);
        }

        public byte[] HashBlock(DateTime timestamp, byte[] previousHash, byte[] data)
        {
            byte[] toHash = Encoding.UTF8.GetBytes(timestamp.Ticks.ToString()).Concat(previousHash).Concat(data).ToArray<byte>();

            byte[] hash = new byte[32];
            using (var hasher = SHA256.Create())
            {
                return hasher.ComputeHash(toHash);
            }
        }

        public byte[] HashBlock(Block block)
        {
            return HashBlock(block.Timestamp, block.PreviousHash, block.Data);
        }
    }
}
