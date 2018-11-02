using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace FhirStarter.Inferno.WebAPI.Config
{
    public static class FhirStarterSettingsHelper
    {
        private const string FhirStarterSettingsFhirServiceAssemblies = "FhirStarterSettings:FhirServiceAssemblies";

        public static ICollection<Assembly> GetFhirServiceAssemblies(IConfigurationRoot fhirStarterSettings)
        {
            var assemblyNames = GetArrayFromSettings(fhirStarterSettings);
            if (assemblyNames.Any())
            {
                return  assemblyNames.Select(assemblyName => Assembly.Load(assemblyName.Trim())).ToList();
            }
            throw new ArgumentException($"The setting {FhirStarterSettingsFhirServiceAssemblies} in FhirStarterSettings.json is empty");
        }

        private static ICollection<string> GetArrayFromSettings(IConfigurationRoot fhirStarterSettings)
        {
            var assemblyNames = fhirStarterSettings.GetValue<string[]>(FhirStarterSettingsFhirServiceAssemblies);
            if (assemblyNames != null && assemblyNames.Any())
            {
                return assemblyNames;
            }
            return new List<string>();
        }
    }
}
