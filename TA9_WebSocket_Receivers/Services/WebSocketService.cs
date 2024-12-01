using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Text;
using TA9_WebSocket_Receiver.MessageDB;
using TA9_WebSocket_Receiver.Models;

namespace WebSocketServerExample.Services
{
    public class WebSocketService
    {
        private readonly MessageDbContext _dbContext;

        public WebSocketService(MessageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleWebSocketConnection(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            // Accept WebSocket request
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[1024];

            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {message}");

                var newMessage = new Message
                {
                    Content = message,
                    Timestamp = DateTime.UtcNow
                };

                _dbContext.Messages.Add(newMessage);
                await _dbContext.SaveChangesAsync();

                // Echo the message back to the client
                var response = Encoding.UTF8.GetBytes($"Server echo: {message}");
                await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
