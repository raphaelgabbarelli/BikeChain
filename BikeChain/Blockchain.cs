using System;
using System.Collections.Generic;
using System.Text;

namespace BikeChain
{
    public class Blockchain
    {
        private List<Block> blocks;

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
    }
}
