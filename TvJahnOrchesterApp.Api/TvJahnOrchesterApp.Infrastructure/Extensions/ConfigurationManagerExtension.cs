using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Infrastructure.Extensions
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
