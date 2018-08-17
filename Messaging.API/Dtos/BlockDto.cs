using System.ComponentModel.DataAnnotations;

namespace Messaging.API.Dtos
{
    public class BlockDto
    {
        [Required]
        public string BlockingUsername { get; set; }
        [Required]
        public string BlockedUsername { get; set; }
    }
}