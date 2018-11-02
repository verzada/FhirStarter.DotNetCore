using System;
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

        #region Assembly

        

        private static void RegisterServices(IApplicationBuilder app, IConfigurationRoot fhirStarterSettings)
        {
            var fhirService = typeof(IFhirService);
            var fhirStructureDefinition = typeof(AbstractStructureDefinitionService);

            var serviceTypes = new List<TypeInitializer>
            {
                new TypeInitializer(true, fhirService, nameof(IFhirService)),
                new TypeInitializer(true, fhirStructureDefinition, nameof(AbstractStructureDefinitionService))
            };

            var fhirServiceAssemblies = FhirStarterSettingsHelper.GetFhirServiceAssemblies(fhirStarterSettings);

            foreach (var asm in fhirServiceAssemblies)
            {
                var types = asm.GetTypes();
                foreach (var classType in asm.GetTypes())
                {
                    BindIFhirServices(app, serviceTypes, classType);
                }
            }
        }

        private static void BindIFhirServices(IApplicationBuilder app, List<TypeInitializer> serviceTypes, Type classType)
        {
            var serviceType = FindType(serviceTypes, classType);
            if (serviceType != null)
            {
                if (serviceType.Name.Equals(nameof(IFhirService)))
                {
                    var instance = (IFhirService)Activator.CreateInstance(classType);
                    //app.Bind<IFhirService>().ToConstant(instance);
                    //_amountOfInitializedIFhirServices++;
                }
                //else if (serviceType.Name.Equals(nameof(IFhirMockupService)))
                //{
                //    var instance = (IFhirMockupService)Activator.CreateInstance(classType);
                //    kernel.Bind<IFhirMockupService>().ToConstant(instance);
                //    _amountOfInitializedIFhirMockupServices++;
                //}
                //else if (serviceType.Name.Equals(nameof(AbstractStructureDefinitionService)))
                //{
                //    var structureDefinitionService = (AbstractStructureDefinitionService)Activator.CreateInstance(classType);
                //    kernel.Bind<AbstractStructureDefinitionService>().ToConstant(structureDefinitionService);
                //    var validator = structureDefinitionService.GetValidator();
                //    if (validator != null)
                //    {
                //        var profileValidator = new ProfileValidator(validator);
                //        kernel.Bind<ProfileValidator>().ToConstant(profileValidator);
                //    }
                //    _amountOfIFhirStructureDefinitionsInitialized++;
                //}
            }
        }

        private static TypeInitializer FindType(List<TypeInitializer> serviceTypes, Type classType)
        {
            foreach (var service in serviceTypes)
            {
                if (service.ServiceType.IsAssignableFrom(classType) && !classType.IsInterface && !classType.IsAbstract)
                    return service;
            }
            return null;
        }

        #endregion Assembly


        private static void SetupLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Debug);
        }
    }
}
