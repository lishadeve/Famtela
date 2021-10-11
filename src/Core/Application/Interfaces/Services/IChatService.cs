//using Famtela.Application.Interfaces.Chat;
//using Famtela.Application.Models.Chat;
using Famtela.Application.Responses.Identity;
using Famtela.Domain.Application.Interfaces.Chat;
using Famtela.Domain.Application.Models.Chat;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}