using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Repositories
{
    public interface ICowRepository
    {
        Task<bool> IsBreedUsed(int breedId);
        Task<bool> IsColorUsed(int colorId);
        Task<bool> IsStatusUsed(int statusId);
        Task<bool> IsTagUsed(int tagId);
    }
}
