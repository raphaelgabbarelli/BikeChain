using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace BikeChain.Tests
{
    public class BlockRepoTests
    {
        [Fact]
        public void GetGenesisBlock()
        {
            
            DateTime timestamp = new DateTime(1998, 6, 7);
            byte[] prevHash = StringToByteArray("916ea4dc3717b0c46156ed9382e9dac329212d3d1bdd30516bdabef670798feb");
            byte[] thisHash = StringToByteArray("18f34ff0ff9b8a8ba44d2f73c26e3ba179acf62225ab87cf3afb86dcb95b34d4");

            BlockRepository systemUnderTest = new BlockRepository();
            Block genesis = systemUnderTest.CreateGenesisBlock();
            string representation = genesis.ToString();

            Assert.Contains(timestamp.Ticks.ToString(), representation);
            Assert.Equal(prevHash, genesis.PreviousHash);
            Assert.Equal(thisHash, genesis.Hash);
        }

        static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
