using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BikeChain.API.WebSockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BikeChain.API
{
    public class Program
    {
        public static Blockchain Blockchain = new Blockchain();
        public static List<string> Peers = new List<string>();
        public static List<WebSocketWrapper> connectedClients = new List<WebSocketWrapper>();
        
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

            string peers = configuration["peers"];
            Console.WriteLine(peers);
            if(!string.IsNullOrEmpty(peers))
            {
                foreach (var peer in peers.Split(','))
                {
                    WebSocketWrapper peerConnection = WebSocketWrapper.Create(peer);
                    peerConnection.OnConnect(wsw =>
                    {
                        Console.WriteLine($"connected to {peer}");
                    });
                    peerConnection.OnMessage((message, wsw) =>
                    {
                        Console.WriteLine(Convert.ToBase64String(message));
                    });

                    connectedClients.Add(peerConnection);
                    peerConnection.Connect();
                    peerConnection.OnDisconnect(wsw =>
                    {
                        Console.WriteLine($"Disconnecting...");
                        connectedClients.Remove(wsw);
                    });
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
