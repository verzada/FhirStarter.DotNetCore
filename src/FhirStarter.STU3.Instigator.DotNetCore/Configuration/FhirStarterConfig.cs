using System;
using System.Collections.Generic;
using System.Reflection;
using FhirStarter.STU3.Detonator.DotNetCore.Interface;
using FhirStarter.STU3.Instigator.DotNetCore.Helper;
using FhirStarter.STU3.Instigator.DotNetCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FhirStarter.STU3.Instigator.DotNetCore.Configuration
{
    public static class FhirStarterConfig
    {
        private static int _amountOfInitializedIFhirServices;
        private static int _amountOfIFhirStructureDefinitionsInitialized;
        private static int _amountOfInitializedIFhirMockupServices;

        public static void SetupFhir(IServiceCollection services, ILoggerFactory loggerFactory,
            IHttpContextAccessor contextAccessor, IConfigurationRoot appSettings, IConfigurationRoot fhirStarterSettings)
        {
            SetupStandardFhirController(services, fhirStarterSettings);
           SetupLogging(loggerFactory);
            RegisterServices(services, fhirStarterSettings);
        }

        private static void SetupStandardFhirController(IServiceCollection services, IConfigurationRoot fhirStarterSettings)
        {
            services.Configure<FhirStarterSettings>(fhirStarterSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddMvc(config =>
                {
                    // Add XML Content Negotiation
                    config.RespectBrowserAcceptHeader = true;
                    config.InputFormatters.Add(new XmlSerializerInputFormatter());
                    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                })
                //.AddXmlSerializerFormatters()
                //.AddXmlDataContractSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static void SetupFhirServices(IServiceCollection services,IConfigurationRoot appSettings, IConfigurationRoot fhirStarterSettings)
        {
            RegisterServices(services, fhirStarterSettings);
        }

        #region Assembly

        

        private static void RegisterServices(IServiceCollection services, IConfigurationRoot fhirStarterSettings)
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
                foreach (var classType in types)
                {
                    BindIFhirServices(services, serviceTypes, classType);
                }
            }
        }

        private static void BindIFhirServices(IServiceCollection services, List<TypeInitializer> serviceTypes, Type classType)
        {
            var serviceType = FindType(serviceTypes, classType);
            if (serviceType != null)
            {
                if (serviceType.Name.Equals(nameof(IFhirService)))
                {
                    var instance = (IFhirService)Activator.CreateInstance(classType);
                   services.Add(new ServiceDescriptor(typeof(IFhirService), instance));
                    //app.Bind<IFhirService>().ToConstant(instance);
                    _amountOfInitializedIFhirServices++;
                }
                else if (serviceType.Name.Equals(nameof(IFhirMockupService)))
                {
                    var instance = (IFhirMockupService)Activator.CreateInstance(classType);
                    services.Add(new ServiceDescriptor(typeof(IFhirMockupService), instance));
                    //kernel.Bind<IFhirMockupService>().ToConstant(instance);
                    _amountOfInitializedIFhirMockupServices++;
                }
                //else if (serviceType.Name.Equals(nameof(AbstractStructureDefinitionService)))
                //{
                //    var structureDefinitionService = (AbstractStructureDefinitionService)Activator.CreateInstance(classType);
                //  //  kernel.Bind<AbstractStructureDefinitionService>().ToConstant(structureDefinitionService);
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
