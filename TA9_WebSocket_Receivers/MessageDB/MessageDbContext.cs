using Microsoft.EntityFrameworkCore;
using System;
using TA9_WebSocket_Receiver.Models;

namespace TA9_WebSocket_Receiver.MessageDB
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }  
    }
}
