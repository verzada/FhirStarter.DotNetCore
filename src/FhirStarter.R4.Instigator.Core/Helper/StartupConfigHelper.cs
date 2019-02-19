using System;
using Microsoft.Extensions.Configuration;

namespace FhirStarter.R4.Instigator.Core.Helper
{
    public static class StartupConfigHelper
    {
        public static IConfigurationRoot BuildConfigurationFromJson(string basePath, string settingsFilename)
        {
            if (!string.IsNullOrEmpty(basePath) && !string.IsNullOrEmpty(settingsFilename))
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile(settingsFilename)
                    .Build();

                return configuration;
            }
            throw new ArgumentNullException($"{nameof(basePath)} or {nameof(settingsFilename)} input to {nameof(BuildConfigurationFromJson)} cannot be null or empty.");
        }
    }
}
