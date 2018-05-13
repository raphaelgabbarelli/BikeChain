using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BikeChain.dto;

namespace BikeChain.API.Controllers
{
    public class HomeController : Controller
    {
        private Blockchain blockchain = Program.Blockchain;

        public JsonResult Blocks()
        {
            if(blockchain != null && blockchain.IsValidChain())
            { 
                return Json(new { status = "ok", blockchain = (BlockchainDto)blockchain });
            }
            return Json(new { status = "empty" });
        }
    }
}
