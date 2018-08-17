using System.Collections.Generic;
using System.Threading.Tasks;
using Messaging.API.Models;

namespace Messaging.API.Repositories
{
    public interface IMessageRepository
    {
     

        Task<bool> CreateMessage(Message message);

        Task<Message> GetMessage(int id);

        

    }
}