﻿using BikeChain.Tests.Utils;
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

        
    }
}
