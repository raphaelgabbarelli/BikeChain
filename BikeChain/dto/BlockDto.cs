using System;
using System.Collections.Generic;
using System.Text;

namespace BikeChain.dto
{
    public class BlockDto
    {
        public BlockDto(Block block)
        {
            Timestamp = (block.Timestamp - new DateTime(1970, 1, 1)).TotalMilliseconds;
            PreviousHash = BitConverter.ToString(block.PreviousHash).Replace("-", "").ToLower();
            Hash = BitConverter.ToString(block.Hash).Replace("-", "").ToLower();
            Data = Convert.ToBase64String(block.Data);
        }

        /// <summary>
        /// unix timestamp in milliseconds
        /// </summary>
        public double Timestamp { get; private set; }

        /// <summary>
        /// Hex string representation of the sha256 hash of the previous block
        /// </summary>
        public string PreviousHash { get; private set; }

        /// <summary>
        /// Hex string representation of the sha256 hash of this block
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Base64 representation of the data of this block
        /// </summary>
        public string Data { get; private set; }
    }
}
