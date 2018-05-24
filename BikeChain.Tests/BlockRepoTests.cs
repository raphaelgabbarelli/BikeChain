using BikeChain.Tests.Utils;
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
            string prevHash = "916ea4dc3717b0c46156ed9382e9dac329212d3d1bdd30516bdabef670798feb";
            byte[] prevHashBytes = Transformations.StringToByteArray(prevHash);
            string thisHash = "18f34ff0ff9b8a8ba44d2f73c26e3ba179acf62225ab87cf3afb86dcb95b34d4";
            byte[] thisHashBytes = Transformations.StringToByteArray(thisHash);

            BlockRepository systemUnderTest = new BlockRepository();
            Block genesis = systemUnderTest.CreateGenesisBlock();
            string representation = genesis.ToString();
            
            Assert.Contains(timestamp.Ticks.ToString(), representation);
            Assert.Equal(prevHashBytes, genesis.PreviousHash);
            Assert.Contains(prevHash, representation);
            Assert.Equal(thisHashBytes, genesis.Hash);
            Assert.Contains(thisHash, representation);
        }

        [Fact]
        public void Mine2ndBlock()
        {
            BlockRepository blockRepo = new BlockRepository();
            Block genesis = blockRepo.CreateGenesisBlock();

            byte[] data = Encoding.UTF8.GetBytes("fakedata");
            Block newBlock = blockRepo.MineBlock(genesis, data);

            Assert.NotNull(newBlock);
            Assert.Equal(genesis.Hash, newBlock.PreviousHash);
            Assert.Contains(Convert.ToBase64String(data), newBlock.ToString());
            Assert.Equal(1, newBlock.Difficulty);
            Assert.Equal(0, newBlock.Hash[0]);
        }

        [Fact]
        public void IncreaseDifficulty()
        {
            BlockRepository blockRepo = new BlockRepository();
            Block genesis = blockRepo.CreateGenesisBlock(); // using genesis as template
            genesis.Difficulty = 1;
            genesis.Timestamp = DateTime.UtcNow.AddSeconds(-1);

            byte[] data = Encoding.UTF8.GetBytes("fakedata");
            Block newBlock = blockRepo.MineBlock(genesis, data);

            Assert.NotNull(newBlock);
            Assert.Equal(genesis.Hash, newBlock.PreviousHash);
            Assert.Contains(Convert.ToBase64String(data), newBlock.ToString());
            Assert.Equal(2, newBlock.Difficulty);
            Assert.Equal(0, newBlock.Hash[0]);
            Assert.Equal(0, newBlock.Hash[1]);
        }

        [Fact]
        public void ReduceDifficulty()
        {
            BlockRepository blockRepo = new BlockRepository();
            Block genesis = blockRepo.CreateGenesisBlock(); // using genesis as template
            genesis.Difficulty = 3;
            genesis.Timestamp = DateTime.UtcNow.AddSeconds(-100);

            byte[] data = Encoding.UTF8.GetBytes("fakedata");
            Block newBlock = blockRepo.MineBlock(genesis, data);

            Assert.NotNull(newBlock);
            Assert.Equal(genesis.Hash, newBlock.PreviousHash);
            Assert.Contains(Convert.ToBase64String(data), newBlock.ToString());
            Assert.Equal(2, newBlock.Difficulty);
            Assert.Equal(0, newBlock.Hash[0]);
            Assert.Equal(0, newBlock.Hash[1]);
        }
    }
}
