using AutoMapper;
//using Famtela.Application.Interfaces.Chat;
//using Famtela.Application.Models.Chat;
using Famtela.Domain.Application.Interfaces.Chat;
using Famtela.Domain.Application.Models.Chat;
using Famtela.Domain.Entities.Identity;
//using Famtela.Infrastructure.Models.Identity;

namespace Famtela.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<FamtelaUser>>().ReverseMap();
        }
    }
}