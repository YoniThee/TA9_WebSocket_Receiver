using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TA9_WebSocket_Receiver.MessageDB;
using TA9_WebSocket_Receiver.Models;

namespace WebSocketServerExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly MessageDbContext _dbContext;

        public DataController(MessageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var messages = await _dbContext.Messages.ToListAsync();
            return Ok(messages);
        }
    }

}

