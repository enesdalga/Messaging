using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messaging.API.Dtos
{
    public class MessageDto
    {
        public MessageDto()
        {
            Date=DateTime.Now;
        }

        [Required]
        public string SenderUsername { get; set; }
        [Required]
        public string ReceiverUsername { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
