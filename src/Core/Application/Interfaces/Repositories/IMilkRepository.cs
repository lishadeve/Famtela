using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Repositories
{
    public interface IMilkRepository
    {
        Task<bool> IsCowUsed(int cowId);
    }
}
