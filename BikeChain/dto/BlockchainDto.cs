using System;
using System.Collections.Generic;
using System.Text;

namespace BikeChain.dto
{
    [Serializable]
    public class BlockchainDto
    {
        public BlockchainDto()
        {
            Blocks = new List<BlockDto>();
        }

        public List<BlockDto> Blocks { get; private set; }
    }
}
