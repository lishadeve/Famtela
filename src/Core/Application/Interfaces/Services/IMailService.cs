using Famtela.Application.Requests.Mail;
using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}