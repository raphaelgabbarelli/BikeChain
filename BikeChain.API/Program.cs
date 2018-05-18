using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using BikeChain.API.WebSockets;
using BikeChain.dto;
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
                        Blockchain bc = DeserializeBlockchain(message);
                        Console.WriteLine($"LENGTH: {bc.Length} blocks - Last Block Timestamp {bc.LastBlock.Timestamp}");
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

        public static void BroadcastBlockchain()
        {
            byte[] data = SerializeBlockchain(Blockchain);
            foreach (var peer in connectedClients)
            {
                peer.SendMessage(data);
            }
        }

        // TODO: this code is also in BlockchainHandler - CONSOLIDATE!
        private static byte[] SerializeBlockchain(Blockchain blockchain)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, (BlockchainDto)blockchain);
            return memoryStream.ToArray();
        }
        private static Blockchain DeserializeBlockchain(byte[] data)
        {
            var binFormatter = new BinaryFormatter();
            var memStream = new MemoryStream(data);
            memStream.Seek(0, SeekOrigin.Begin);
            var blockchain = binFormatter.Deserialize(memStream) as BlockchainDto;

            return new Blockchain(blockchain);
        }
    }
}
