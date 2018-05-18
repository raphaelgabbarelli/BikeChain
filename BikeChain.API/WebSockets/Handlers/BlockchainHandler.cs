using BikeChain.dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
            byte[] allData = buffer;
            while (!result.EndOfMessage)
            {
                Console.WriteLine("round");
                var arrSegment = new ArraySegment<byte>(new byte[1024]);
                result = await socket.ReceiveAsync(arrSegment, CancellationToken.None).ConfigureAwait(false);
                allData = allData.Concat(arrSegment.Array).ToArray();
                Console.WriteLine($"arrSeg length: {arrSegment.Count} - alldata length: {allData.Length}");
            }

            string base64Message = Convert.ToBase64String(allData);
            Console.WriteLine($"message length: {allData.Length} - is end of message: {result.EndOfMessage} \n{base64Message}");
            
            Blockchain bc = DeserializeBlockchain(allData);
            Console.WriteLine($"LENGTH: {bc.Length} blocks - Last Block Timestamp {bc.LastBlock.Timestamp}");
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

        private Blockchain DeserializeBlockchain(byte[] data)
        {
            var binFormatter = new BinaryFormatter();
            var memStream = new MemoryStream(data);
            var blockchain = binFormatter.Deserialize(memStream) as BlockchainDto;

            return new Blockchain(blockchain);
        }
    }
}
