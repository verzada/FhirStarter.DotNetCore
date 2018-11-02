using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace FhirStarter.Inferno.WebAPI.Config
{
    public static class AssemblyLoaderHelper
    {
        private const string FhirStarterSettingsFhirServiceAssemblies = "FhirStarterSettings:FhirServiceAssemblies";

        public static ICollection<Assembly> GetFhirServiceAssemblies(IConfigurationRoot fhirStarterSettings)
        {
        //    var assemblyNames = ConfigurationManager<>.AppSettings["FhirServiceAssemblies"];
            //var assemblyNamesSplit = assemblyNames.Split(',');
            //return assemblyNamesSplit.Select(assemblyName => Assembly.Load(assemblyName.Trim())).ToList();
            return new List<Assembly>();
        }

        private static ICollection<string> GetArrayFromSettings(IConfigurationRoot fhirStarterSettings)
        {

            var fhirServiceAssembliesSection = fhirStarterSettings.GetSection(FhirStarterSettingsFhirServiceAssemblies);
            if (fhirServiceAssembliesSection != null)
            {
                var assemblyArray = fhirServiceAssembliesSection.Get<string[]>();
                return assemblyArray;
            }
            throw new ArgumentException($"The setting {FhirStarterSettingsFhirServiceAssemblies} in FhirStarterSettings.json is empty");
      
        }
    }
}
