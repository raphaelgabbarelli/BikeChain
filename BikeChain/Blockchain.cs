using BikeChain.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeChain
{
    [Serializable]
    public class Blockchain
    {
        private List<Block> blocks;

        [NonSerialized]
        private BlockRepository blockRepo = new BlockRepository();

        public Blockchain()
        {
            blocks = new List<Block>();
            blocks.Add(blockRepo.CreateGenesisBlock());
        }

        /// <summary>
        /// Creates and adds a new block to the blockchain
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The newly minted block</returns>
        public Block AddBlock(byte[] data)
        {
            Block newBlock = blockRepo.MineBlock(LastBlock, data);
            blocks.Add(newBlock);
            return newBlock;
        }

        /// <summary>
        /// returns a copy of the last block
        /// </summary>
        public Block LastBlock
        {
            get
            {
                return blocks[blocks.Count - 1].Clone() as Block;
            }
        }
        
        public int Length
        {
            get
            {
                return blocks.Count;
            }
        }

        public bool IsValidChain()
        {
            var blockRepo = new BlockRepository();
            if (!blocks[0].Equals(blockRepo.CreateGenesisBlock()))
            {
                return false;
            }

            for (int i = 1; i < blocks.Count; i++)
            {
                Block currentBlock = blocks[i];
                Block previousBlock = blocks[i - 1];

                if(!currentBlock.PreviousHash.SequenceEqual(previousBlock.Hash) ||
                    !currentBlock.Hash.SequenceEqual(blockRepo.HashBlock(currentBlock)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Replaces the blocks of the blockchain with a longer and valid incoming blockchain
        /// </summary>
        /// <param name="newBlockchain">incoming blockchain</param>
        /// <returns>true if the operation is successful. 
        /// In case of invalid incoming chain: 
        /// <paramref name="incomingChainTooShort"/> indicates that <paramref name="newBlockchain"/> is shorter or of equal length
        /// <paramref name="incomingChainInvalid" /> indicates that <paramref name="newBlockchain"/> is invalid
        /// </returns>
        public (bool result, bool? incomingChainTooShort, bool? incomingChainInvalid) ReplaceBlocks(Blockchain newBlockchain)
        {
            if(newBlockchain.Length <= Length) { return (false, true, null); }
            if(!newBlockchain.IsValidChain()) { return (false, false, true); }

            blocks = newBlockchain.blocks;

            return (true, null, null);
        }

        public static explicit operator BlockchainDto(Blockchain blockchain)
        {
            BlockchainDto dto = new BlockchainDto();

            foreach (var block in blockchain.blocks)
            {
                dto.Blocks.Add(new BlockDto(block));
            }

            return dto;
        }
    }
}
