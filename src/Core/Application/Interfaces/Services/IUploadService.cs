using Famtela.Application.Requests;

namespace Famtela.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}