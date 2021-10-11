using System.Linq;
using Famtela.Shared.Constants.Localization;
using Famtela.Shared.Settings;

namespace Famtela.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}