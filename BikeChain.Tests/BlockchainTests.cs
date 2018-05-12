using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BikeChain.Tests
{
    public class BlockchainTests
    {
        [Fact]
        public void CheckGenesis()
        {
            Blockchain bc = new Blockchain();

            var lastBlock = bc.LastBlock;   // deep copy of last block, which has to be the genesis block

            Assert.Equal(new DateTime(1998, 6, 7).Ticks, lastBlock.Timestamp.Ticks);
            string representation = lastBlock.ToString();
            Assert.Contains("916ea4dc3717b0c46156ed9382e9dac329212d3d1bdd30516bdabef670798feb", representation);
            Assert.Contains("18f34ff0ff9b8a8ba44d2f73c26e3ba179acf62225ab87cf3afb86dcb95b34d4", representation);
        }
    }
}
