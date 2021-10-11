using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Interfaces.Common;
using Famtela.Application.Requests.Identity;
using Famtela.Application.Responses.Identity;
using Famtela.Shared.Wrapper;

namespace Famtela.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}