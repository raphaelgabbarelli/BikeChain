using BikeChain.dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BikeChain.API.WebSockets.Handlers
{
    public class BlockchainHandler : WebSocketHandler
    {
        public BlockchainHandler(WebSocketConnectionManager connectionManager)
            : base(connectionManager) { }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            Console.WriteLine($"Incoming connection from {WebSocketConnectionManager.GetId(socket)}");
            
            await SendMessageAsync(socket, SerializeBlockchain(Program.Blockchain));
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            await SendMessageAsync(socket, "sending back - " + Encoding.UTF8.GetString(buffer));
        }

        public async Task BroadcastBlockchain()
        {
            Console.WriteLine("broadcasting blockchain");

            await SendMessageToAllAsync(SerializeBlockchain(Program.Blockchain));
        }

        private byte[] SerializeBlockchain(Blockchain blockchain)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, (BlockchainDto)blockchain);
            return memoryStream.ToArray();
        }
    }
}
