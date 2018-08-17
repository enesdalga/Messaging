using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging.API.Data;
using Messaging.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MessagingContext _context;

        public UserRepository(MessagingContext context)
        {
            _context = context;
        }

        public async Task<bool> BlockUser(BlockList blockList)
        {
            await _context.BlockLists.AddAsync(blockList);
            if ((await _context.SaveChangesAsync()) == 1)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<Message>> GetMessages(int userId)
        {
            return await _context.Messages
                        .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                        .Include(s => s.Sender)
                        .Include(r => r.Receiver)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int senderId, int receiverId)
        {
            return await _context.Messages
                        .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId || x.ReceiverId == senderId && x.SenderId == receiverId)
                        .Include(s => s.Sender)
                        .Include(r => r.Receiver)
                        .ToListAsync();
        }

        public async Task<int> GetUserId(string username)
        {
            return (await _context.Users.FirstOrDefaultAsync(x => x.Username == username)).UserId;
        }

        public async Task<bool> UserExistsByUsername(string username)
        {
            return (await _context.Users.AnyAsync(x => x.Username == username));
        }

        public async Task<bool> UserExistsByUserId(int userId)
        {
            return (await _context.Users.AnyAsync(x => x.UserId == userId));
        }

        public async Task<bool> IsBlocked(string SenderUsername, string RecevierUsername)
        {
            return (await _context.BlockLists.AnyAsync(x => x.BlockingUser.Username == RecevierUsername && x.BlockedUser.Username==SenderUsername));
        }
    }
}