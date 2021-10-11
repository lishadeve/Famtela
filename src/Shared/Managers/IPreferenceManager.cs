using Famtela.Shared.Settings;
using System.Threading.Tasks;
using Famtela.Shared.Wrapper;

namespace Famtela.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}