using Famtela.Application.Interfaces.Common;

namespace Famtela.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}