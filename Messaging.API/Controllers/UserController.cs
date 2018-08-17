using System;
using System.Collections.Generic;
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
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepo, IMapper mapper, ILogger<UserController> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{userId}/messages")]
        public async Task<IActionResult> GetMessages(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                _logger.LogInformation("Unauthorized request");
                return Unauthorized();
            }

            var messages = await _userRepo.GetMessages(userId);

            if (messages == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<List<MessageDto>>(messages);

            return Ok(result);
        }

        [HttpGet("{userId}/messagethread/{receiverId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int receiverId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                _logger.LogInformation("Unauthorized request");
                return Unauthorized();
            }

            var IsReceiverExists = await _userRepo.UserExistsByUserId(receiverId);

            if (!IsReceiverExists)
                return BadRequest("User could not found");

            var messages = await _userRepo.GetMessageThread(userId, receiverId);


            if (messages == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<List<MessageDto>>(messages);

            return Ok(result);
        }

        [HttpPost()]
        [Route("block")] 
        public async Task<IActionResult> BlockUser(BlockDto blockDto)
        {
            var IsUserExists = await _userRepo.UserExistsByUsername(blockDto.BlockedUsername);

            if (!IsUserExists)
                return BadRequest("User could not found");

            var blockList = new BlockList
            {
                BlockingUserId = await _userRepo.GetUserId(blockDto.BlockingUsername),
                BlockedUserId = await _userRepo.GetUserId(blockDto.BlockedUsername)
            };


            if (await _userRepo.BlockUser(blockList))
                return StatusCode(201);

            _logger.LogError("Failed to blocking user");
            return BadRequest("Failed to blocking user");
        }
    }
}