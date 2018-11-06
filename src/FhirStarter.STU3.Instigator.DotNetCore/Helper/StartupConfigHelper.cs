using System;
using Microsoft.Extensions.Configuration;

namespace FhirStarter.STU3.Instigator.DotNetCore.Helper
{
    public static class StartupConfigHelper
    {
        public static IConfigurationRoot BuildConfiguration(string basePath, string settingsFilename)
        {
            if (!string.IsNullOrEmpty(basePath) && !string.IsNullOrEmpty(settingsFilename))
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile(settingsFilename)
                    .Build();

                return configuration;
            }
            throw new ArgumentNullException($"{nameof(basePath)} or {nameof(settingsFilename)} input to {nameof(BuildConfiguration)} cannot be null or empty.");
        }
    }
}
