using System.Collections.Generic;
using FhirStarter.Bonfire.DotNetCore.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FhirStarter.Inferno.WebAPI.Config
{
    public static class FhirStarterConfig
    {
        public static void SetupFhir(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IHttpContextAccessor contextAccessor, IConfigurationRoot appSettings, IConfigurationRoot fhirStarterSettings)
        {
            
           SetupLogging(loggerFactory);
            RegisterServices(app, fhirStarterSettings);
        }


        private static void RegisterServices(IApplicationBuilder app, IConfigurationRoot fhirStarterSettings)
        {
            var fhirService = typeof(IFhirService);
            var fhirStructureDefinition = typeof(AbstractStructureDefinitionService);

            var serviceTypes = new List<TypeInitializer>
            {
                new TypeInitializer(true, fhirService, nameof(IFhirService)),
                new TypeInitializer(true, fhirStructureDefinition, nameof(AbstractStructureDefinitionService))
            };

            //var serviceAssemblies = AssemblyLoaderHelper.GetFhirServiceAssemblies();
            //foreach (var asm in serviceAssemblies)
            //{
            //    var types = asm.GetTypes();
            //    foreach (var classType in asm.GetTypes())
            //    {
            //        BindIFhirServices(kernel, serviceTypes, classType);
            //    }
            //}
        }

        private static void SetupLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Debug);
        }
    }
}
