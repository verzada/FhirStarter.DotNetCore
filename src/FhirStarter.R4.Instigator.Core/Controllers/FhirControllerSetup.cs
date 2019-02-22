using System;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Instigator.Core.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FhirStarter.R4.Instigator.Core.Controllers
{
    [Route("fhir"), EnableCors]
    public partial class FhirController
    {
        private readonly AbstractStructureDefinitionService _abstractStructureDefinitionService;
        private ILogger<IFhirService> _log;
        private readonly IConfigurationRoot _fhirStarterSettings;

        private bool ValidationEnabled;

        private bool IsMockupEnabled;
        //private AcceptHeaderAttribute _acceptHeaderAttributes;

        public FhirController(ILogger<IFhirService> loggerFactory, IConfigurationRoot fhirStarterSettings)
        {
            _log = loggerFactory;
            //_acceptHeaderAttributes = new AcceptHeaderAttribute("application/json","application/xml");
            _fhirStarterSettings = fhirStarterSettings;

            ValidationEnabled = GetSetting(_fhirStarterSettings, "EnableValidation");
            IsMockupEnabled = GetSetting(_fhirStarterSettings, "MockupEnabled");
        }

        private bool GetSetting(IConfigurationRoot fhirStarterSettings, string key)
        {
            var keyValue = fhirStarterSettings.GetSection($"FhirStarterSettings:{key}");
            if (!string.IsNullOrEmpty(keyValue.Value))
            {
                bool.TryParse(keyValue.Value, out var keyBool);
                return keyBool;
            }
            throw new ArgumentException($"The setting {key} must be defined in {nameof(FhirStarterSettings)} with a true / false value");
        }
    }
}
