using System;
using System.Collections.Generic;
using System.Text;

namespace BikeChain
{
    public class Block
    {
        public DateTime Timestamp { get; set; }
        public byte[] PreviousHash { get; set; }
        public byte[] Hash { get; set; }
        public object Data { get; set; }
        
        public Block(DateTime timestamp, byte[] previousHash, byte[] hash, object data)
        {
            Timestamp = timestamp;
            PreviousHash = previousHash;
            Hash = hash;
            Data = data;
        }
    }
}
