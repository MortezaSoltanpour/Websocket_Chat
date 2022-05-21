using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebsocketTraining.SocketManager;

namespace WebsocketTraining.Handlers
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connection) : base(connection)
        {
        }

        public override async Task onConnected(WebSocket socket)
        {
            await base.onConnected(socket);
            var socketId = _connection.GetId(socket);
            await SendMessageToAll($"{socketId } just joined the party ******");
        }


        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = _connection.GetId(socket);
            var message = $"{socketId} said : { Encoding.UTF8.GetString(buffer, 0, result.Count) }" ;
            await SendMessageToAll(message);
        }
    }
}