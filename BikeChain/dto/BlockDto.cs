using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeChain.dto
{
    [Serializable]
    public class BlockDto
    {
        public BlockDto(Block block)
        {
            Timestamp = (long)(block.Timestamp - new DateTime(1970, 1, 1)).TotalMilliseconds;
            PreviousHash = BitConverter.ToString(block.PreviousHash).Replace("-", "").ToLower();
            Hash = BitConverter.ToString(block.Hash).Replace("-", "").ToLower();
            Data = Convert.ToBase64String(block.Data);
            Difficulty = block.Difficulty;
            Nonce = Convert.ToBase64String(block.Nonce);
        }

        /// <summary>
        /// unix timestamp in milliseconds
        /// </summary>
        public long Timestamp { get; private set; }

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

        /// <summary>
        /// Difficulty at which this block has been mined
        /// </summary>
        public int Difficulty { get; set; }

        /// <summary>
        /// Base64 representation of the nonce
        /// </summary>
        public string Nonce { get; set; }

        public static explicit operator Block(BlockDto dto)
        {
            Block block = new Block(new DateTime(1970, 1, 1).AddMilliseconds(dto.Timestamp),
                StringToByteArray(dto.PreviousHash),
                StringToByteArray(dto.Hash),
                Convert.FromBase64String(dto.Data),
                dto.Difficulty,
                Convert.FromBase64String(dto.Nonce)
                );
            return block;
        }

        // TODO: move this somewhere else, it's already copied in tests!!!
        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
    }
}
