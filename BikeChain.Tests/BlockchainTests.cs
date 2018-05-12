using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xunit;

namespace BikeChain.Tests
{
    public class BlockchainTests
    {
        [Fact]
        public void CheckGenesis()
        {
            Blockchain blockchain = new Blockchain();

            var lastBlock = blockchain.LastBlock;   // deep copy of last block, which has to be the genesis block

            Assert.Equal(new DateTime(1998, 6, 7).Ticks, lastBlock.Timestamp.Ticks);
            string representation = lastBlock.ToString();
            Assert.Contains("916ea4dc3717b0c46156ed9382e9dac329212d3d1bdd30516bdabef670798feb", representation);
            Assert.Contains("18f34ff0ff9b8a8ba44d2f73c26e3ba179acf62225ab87cf3afb86dcb95b34d4", representation);
        }

        [Fact]
        public void ValidateValidChain()
        {
            Blockchain blockchain = new Blockchain();

            Assert.True(Blockchain.IsValidChain(blockchain));

            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));

            Assert.True(Blockchain.IsValidChain(blockchain));
        }

        [Fact]
        public void RejectTamperedChain()
        {
            Blockchain blockchain = new Blockchain();
            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));
            Assert.True(Blockchain.IsValidChain(blockchain));   // not tampered yet

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, blockchain);

            memoryStream.Seek(-2, SeekOrigin.End);
            memoryStream.WriteByte(142);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var tamperedBlockchain = binaryFormatter.Deserialize(memoryStream) as Blockchain;
            Assert.NotNull(tamperedBlockchain);
            Assert.False(Blockchain.IsValidChain(tamperedBlockchain));
        }
    }
}
