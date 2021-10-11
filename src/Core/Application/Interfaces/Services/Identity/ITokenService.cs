using Famtela.Application.Interfaces.Common;
using Famtela.Application.Requests.Identity;
using Famtela.Application.Responses.Identity;
using Famtela.Shared.Wrapper;
using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}