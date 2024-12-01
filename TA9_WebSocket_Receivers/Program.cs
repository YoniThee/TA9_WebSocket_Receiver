using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSocketServerExample.Controllers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using WebSocketServerExample.Services;
using Microsoft.EntityFrameworkCore;
using System;
using TA9_WebSocket_Receiver.MessageDB;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<WebSocketService>();
builder.Services.AddScoped<WebSocketService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<MessageDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
});
var app = builder.Build();

// Enable WebSockets
app.UseWebSockets();
app.MapControllers();

// Handle WebSocket connections
app.Map("/ws", async context =>
{
    var webSocketService = context.RequestServices.GetRequiredService<WebSocketService>();
    await webSocketService.HandleWebSocketConnection(context);
});

// API Controllers
app.MapControllers();

app.Run();

// WebSocket connection handler
//async Task HandleWebSocketConnection(HttpContext context)
//{
//    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//    var buffer = new byte[1024];

//    while (true)
//    {
//        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//        if (result.MessageType == WebSocketMessageType.Close)
//        {
//            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
//            break;
//        }

//        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
//        Console.WriteLine($"Received message: {message}");

//        // Echo the message back to the client
//        var response = Encoding.UTF8.GetBytes($"Server echo: {message}");
//        await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
//    }
//}
