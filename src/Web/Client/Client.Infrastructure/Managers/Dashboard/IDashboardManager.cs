using Famtela.Shared.Wrapper;
using System.Threading.Tasks;
using Famtela.Application.Features.Dashboards.Queries.GetData;

namespace Famtela.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}