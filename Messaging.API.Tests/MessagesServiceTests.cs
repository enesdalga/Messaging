using System;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Messaging.API.Controllers;
using Messaging.API.Dtos;
using Messaging.API.Models;
using Messaging.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Messaging.API.Tests
{
    public class MessagesServiceTests
    {
        private MessagesController _messagesController;
        private Mock<IMessageRepository> _messagesRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<ILogger<MessagesController>> _logger;
        private Mock<IMapper> _mapper;


        public MessagesServiceTests()
        {
            _messagesRepository = new Mock<IMessageRepository>();
            _userRepository = new Mock<IUserRepository>();
            _logger = new Mock<ILogger<MessagesController>>();
            _mapper = new Mock<IMapper>();

            _messagesController = new MessagesController(_messagesRepository.Object, _mapper.Object, _userRepository.Object, _logger.Object);
        }
        [Fact]
        public void GetMessage_InvalidMessageId_ReturnsMessageNotFound()
        {
            
        }
    }
}
