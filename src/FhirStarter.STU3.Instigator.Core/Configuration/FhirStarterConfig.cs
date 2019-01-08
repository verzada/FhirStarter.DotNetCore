using System;
using System.Collections.Generic;
using FhirStarter.STU3.Detonator.Core.Formatters;
using FhirStarter.STU3.Detonator.Core.Interface;
using FhirStarter.STU3.Instigator.Core.Helper;
using FhirStarter.STU3.Instigator.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FhirStarter.STU3.Instigator.Core.Configuration
{
    public static class FhirStarterConfig
    {
        private static int _amountOfInitializedIFhirServices;
        private static int _amountOfIFhirStructureDefinitionsInitialized;
        private static int _amountOfInitializedIFhirMockupServices;

        public static void SetupFhir(IServiceCollection services, IConfigurationRoot fhirStarterSettings, CompatibilityVersion dotNetCoreVersion)
        {
            SetupHeadersAndController(services, fhirStarterSettings, dotNetCoreVersion);
            RegisterServices(services, fhirStarterSettings);
        }

        private static void SetupHeadersAndController(IServiceCollection services, IConfigurationRoot fhirStarterSettings, CompatibilityVersion dotNetCoreVersion)
        {
            services.Configure<FhirStarterSettings>(fhirStarterSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddMvc(options =>
                {
                    options.OutputFormatters.Clear();
                  options.RespectBrowserAcceptHeader = true;
                  options.OutputFormatters.Add(new XmlFhirSerializerOutputFormatter());
                  options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                  options.OutputFormatters.Add(new JsonFhirFormatter());
                })
                .SetCompatibilityVersion(dotNetCoreVersion);
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
    }
}
