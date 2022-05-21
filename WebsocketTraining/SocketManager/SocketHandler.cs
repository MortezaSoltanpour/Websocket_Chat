using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebsocketTraining.SocketManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager _connection { get; set; }


        public SocketHandler(ConnectionManager connection)
        {
            _connection = connection;
        }

        public virtual async Task onConnected(WebSocket socket)
        {
            await Task.Run(() =>
            {
                _connection.AddSocket(socket);
            }
            );
        }

        public virtual async Task onDisconnected(WebSocket socket)
        {
            await _connection.RemoveSocketAsync(_connection.GetId(socket));
        }


        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;


            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, message.Length), WebSocketMessageType.Text,
                true, CancellationToken.None);
        }

        public async Task SendMessage(string id, string message)
        {
            await SendMessage(_connection.GetSocketById(id), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var con in _connection.GetAllConnections())
            {
                await SendMessage(con.Value, message);
            }
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }


  
}
