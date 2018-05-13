using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeChain.API.Models
{ 
    public class MineRequestModel
    {
        /// <summary>
        /// Data to be mined, in Base64 format
        /// </summary>
        public string Data { get; set; }
    }
}
