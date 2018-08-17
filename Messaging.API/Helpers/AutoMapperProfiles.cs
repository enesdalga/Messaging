using AutoMapper;
using Messaging.API.Dtos;
using Messaging.API.Models;

namespace Messaging.API.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(x=>x.ReceiverUsername,y=>y.MapFrom(z=>z.Receiver.Username))
                .ForMember(x=>x.SenderUsername,y=>y.MapFrom(z=>z.Sender.Username));
                
        }
    }
}