using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Requests.Identity;
using Famtela.Application.Responses.Identity;
using Famtela.Shared.Wrapper;

namespace Famtela.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}