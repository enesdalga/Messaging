using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging.API.Data;
using Messaging.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessagingContext _context;

        public MessageRepository(MessagingContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            if ((await _context.SaveChangesAsync()) == 1)
                return true;
            else
                return false;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                .Where(x => x.MessageId == id)
                .Include(s=>s.Sender)
                .Include(r=>r.Receiver).FirstOrDefaultAsync();
        }


    }
}