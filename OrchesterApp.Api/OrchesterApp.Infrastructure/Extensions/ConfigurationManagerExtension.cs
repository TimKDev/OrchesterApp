using Microsoft.Extensions.Configuration;

namespace OrchesterApp.Infrastructure.Extensions
{
    public static class ConfigurationManagerExtension
    {
        public static string? GetValueFromSecretOrConfig(this ConfigurationManager configuration, string key)
        {
            var appSettingsValue = configuration.GetValue<string>(key);
            if (appSettingsValue?[0] == '/')
            {
                appSettingsValue = configuration.GetValue<string>(appSettingsValue.Split('/').Last());
            }
            return appSettingsValue;
        }
    }
}
