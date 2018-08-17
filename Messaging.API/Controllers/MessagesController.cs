using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Messaging.API.Dtos;
using Messaging.API.Models;
using Messaging.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Messaging.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController:ControllerBase
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageRepository messageRepo, IMapper mapper,IUserRepository userRepository, ILogger<MessagesController> logger)
        {
            _messageRepo = messageRepo;
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("{id}",Name="GetMessage")]
        public async Task<IActionResult> GetMessage (int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                _logger.LogInformation("Unauthorized request");
                return Unauthorized();
            }
            
            var message= await _messageRepo.GetMessage(id);

            if (message==null)
            {
                _logger.LogInformation("Message not found");
                return NotFound();
            }

            var result = _mapper.Map<MessageDto>(message);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageDto messageDto)
        {
            if (await _userRepository.GetUserId(messageDto.SenderUsername) != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var IsReceiverExists=await _userRepository.UserExistsByUsername(messageDto.ReceiverUsername);

            if (!IsReceiverExists)
                return BadRequest("User could not found");

            var IsBlocked=await _userRepository.IsBlocked(messageDto.SenderUsername,messageDto.ReceiverUsername);

            if (IsBlocked)
                return BadRequest("You cant send messages because this user blocked you");

            var message =new Message{
                SenderId=await _userRepository.GetUserId(messageDto.SenderUsername),
                ReceiverId= await _userRepository.GetUserId(messageDto.ReceiverUsername),
                Content=messageDto.Content,
                Date=messageDto.Date,
                IsRead=false
            };
            

            if (await _messageRepo.CreateMessage(message))
                return CreatedAtRoute("GetMessage",new {id=message.MessageId},messageDto);

            _logger.LogError("Failed to send message");
            return BadRequest("Failed to send message");
        }
    }
}