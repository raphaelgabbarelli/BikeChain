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

        public static bool IsValidChain(Blockchain chain)
        {
            var blockRepo = new BlockRepository();
            if (!chain.blocks[0].Equals(blockRepo.CreateGenesisBlock()))
            {
                return false;
            }

            for (int i = 1; i < chain.blocks.Count; i++)
            {
                Block currentBlock = chain.blocks[i];
                Block previousBlock = chain.blocks[i - 1];

                if(!currentBlock.PreviousHash.SequenceEqual(previousBlock.Hash) ||
                    !currentBlock.Hash.SequenceEqual(blockRepo.HashBlock(currentBlock)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
