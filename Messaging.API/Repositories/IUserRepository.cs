using System.Collections.Generic;
using System.Threading.Tasks;
using Messaging.API.Models;

namespace Messaging.API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Message>> GetMessages(int userId);
        Task<IEnumerable<Message>> GetMessageThread(int senderId,int receiverId);
        Task<int> GetUserId(string username);
        Task<bool> UserExistsByUsername(string username);
        Task<bool> UserExistsByUserId(int userId);

        Task<bool> BlockUser(BlockList blockList);
        Task<bool> IsBlocked(string SenderUsername,string RecevierUsername);

    }
}