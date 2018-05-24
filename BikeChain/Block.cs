using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeChain
{
    [Serializable]
    public class Block : ICloneable, IEquatable<Block>
    {
        public DateTime Timestamp { get; set; }
        public byte[] PreviousHash { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Data { get; set; }
        public int Difficulty { get; set; }
        public byte[] Nonce { get; set; }
        
        public Block(DateTime timestamp, byte[] previousHash, byte[] hash, byte[] data, int difficulty, byte[] nonce)
        {
            Timestamp = timestamp;
            PreviousHash = previousHash;
            Hash = hash;
            Data = data;
            Difficulty = difficulty;
            Nonce = nonce;
        }

        public override string ToString()
        {
            return $"Block - \n" +
                $"Timestamp - {Timestamp.Ticks}\n" +
                $"Previous hash - {BitConverter.ToString(PreviousHash).Replace("-", "").ToLower()}\n" +
                $"Hash - {BitConverter.ToString(Hash).Replace("-", "").ToLower()}\n" +
                $"Data - {Convert.ToBase64String(Data)}\n" +
                $"Difficulty - {Difficulty}\n" +
                $"Nonce - {BitConverter.ToString(Nonce).Replace("-","").ToLower()}";
        }

        public object Clone()
        {
            return new Block(new DateTime(Timestamp.Ticks), PreviousHash.Clone() as byte[], Hash.Clone() as byte[], Data.Clone() as byte[], Difficulty, Nonce.Clone() as byte[]);
        }

        public bool Equals(Block other)
        {
            bool isEqual = Timestamp == other.Timestamp &&
                PreviousHash.SequenceEqual(other.PreviousHash) &&
                Hash.SequenceEqual(other.Hash) &&
                Data.SequenceEqual(other.Data);
            return isEqual;
        }
    }
}
