namespace Messaging.API.Models
{
    public class BlockList
    {
        public int BlockListId { get; set; }
        public int BlockingUserId { get; set; }
        public User BlockingUser { get; set; }
        public int BlockedUserId { get; set; }
        public User BlockedUser { get; set; }

    }
}