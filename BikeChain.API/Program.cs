using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BikeChain.API
{
    public class Program
    {
        public static Blockchain Blockchain = new Blockchain();
        public static List<string> Peers = new List<string>();
        
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

            string peers = configuration["peers"];
            if(!string.IsNullOrEmpty(peers))
            {
                foreach (var peer in peers.Split(','))
                {
                    Peers.Add(peer);
                    WebSockets.Handlers.BlockchainHandler bch = new WebSockets.Handlers.BlockchainHandler(new WebSockets.WebSocketConnectionManager());
                    
                }
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseConfiguration(configuration)
                .Build();

            host.Run();
        }
    }
}
