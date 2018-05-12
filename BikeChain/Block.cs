using System;
using System.Collections.Generic;
using System.Text;

namespace BikeChain
{
    public class Block : ICloneable
    {
        public DateTime Timestamp { get; set; }
        public byte[] PreviousHash { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Data { get; set; }
        
        public Block(DateTime timestamp, byte[] previousHash, byte[] hash, byte[] data)
        {
            Timestamp = timestamp;
            PreviousHash = previousHash;
            Hash = hash;
            Data = data;
        }

        public override string ToString()
        {
            return $"Block - \n" +
                $"Timestamp - {Timestamp.Ticks}\n" +
                $"Previous hash - {BitConverter.ToString(PreviousHash).Replace("-", "").ToLower()}\n" +
                $"Hash - {BitConverter.ToString(Hash).Replace("-", "").ToLower()}\n" +
                $"Data - {Convert.ToBase64String(Data)}";
        }

        public object Clone()
        {
            return new Block(new DateTime(Timestamp.Ticks), PreviousHash.Clone() as byte[], Hash.Clone() as byte[], Data.Clone() as byte[]);
        }
    }
}
