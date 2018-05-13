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

            Assert.True(blockchain.IsValidChain());

            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));

            Assert.True(blockchain.IsValidChain());
        }

        // TODO: move to fixture
        /// <summary>
        /// Utility method that tampers with a blockchain
        /// </summary>
        /// <param name="blockchain"></param>
        private Blockchain TamperWithChain(Blockchain blockchain)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, blockchain);

            memoryStream.Seek(-2, SeekOrigin.End);
            memoryStream.WriteByte(142);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var tamperedBlockchain = binaryFormatter.Deserialize(memoryStream) as Blockchain;
            return tamperedBlockchain;
        }

        [Fact]
        public void RejectTamperedChain()
        {
            Blockchain blockchain = new Blockchain();
            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));
            Assert.True(blockchain.IsValidChain());   // not tampered yet

            Blockchain tamperedBlockchain = TamperWithChain(blockchain);

            
            Assert.NotNull(tamperedBlockchain);
            Assert.False(tamperedBlockchain.IsValidChain());
        }

        [Fact]
        public void ReplaceWithGoodChain()
        {
            Blockchain blockchain = new Blockchain();
            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));

            Blockchain newBlockchain = new Blockchain();
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("some other transactions"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("latest transactions here"));

            (bool result, bool? tooShort, bool? invalid) = blockchain.ReplaceBlocks(newBlockchain);
            Assert.True(result);
            Assert.False(tooShort.HasValue);
            Assert.False(invalid.HasValue);
        }

        [Fact]
        public void DontReplaceWithShortChain()
        {
            Blockchain blockchain = new Blockchain();
            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));

            Blockchain newBlockchain = new Blockchain();
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));

            (bool result, bool? tooShort, bool? invalid) = blockchain.ReplaceBlocks(newBlockchain);
            Assert.False(result);
            Assert.True(tooShort.HasValue && tooShort.Value);
            Assert.False(invalid.HasValue);
        }

        [Fact]
        public void DontReplaceWithInvalidChain()
        {
            Blockchain blockchain = new Blockchain();
            blockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            blockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));

            Blockchain newBlockchain = new Blockchain();
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("x123 sends 0.1 BIKECOINS to y456"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("more transactions"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("smart contract data"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("some other transactions"));
            newBlockchain.AddBlock(Encoding.UTF8.GetBytes("latest transactions here"));

            Blockchain tamperedBlockchain = TamperWithChain(newBlockchain);

            (bool result, bool? tooShort, bool? invalid) = blockchain.ReplaceBlocks(tamperedBlockchain);
            Assert.False(result);
            Assert.False(tooShort);
            Assert.True(invalid.HasValue && invalid.Value);
        }
    }
}
