using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BikeChain.dto;
using BikeChain.API.Models;

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

        [HttpPost]
        public ActionResult Mine([FromBody]MineRequestModel request)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (String.IsNullOrEmpty(request.Data)) throw new ArgumentNullException("request.Data");

            try
            {
                byte[] data = Convert.FromBase64String(request.Data);
                blockchain.AddBlock(data);
                return RedirectToAction("Blocks");
            }
            catch (FormatException)
            {
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json(new { result = "error", message = "Data is not a valid base64 string" });
            }
            
        }
    }
}
