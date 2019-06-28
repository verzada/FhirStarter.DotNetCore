using System;
using System.Collections.Generic;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Instigator.Core.Helper;
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
        private readonly IConfigurationRoot _appSettings;
        private IEnumerable<IFhirService> _fhirServices;

        private bool ValidationEnabled;

        private bool IsMockupEnabled;
        //private AcceptHeaderAttribute _acceptHeaderAttributes;

        public FhirController(ILogger<IFhirService> loggerFactory, IConfigurationRoot fhirStarterSettings,
            IServiceProvider serviceProvider)
        {
            _log = loggerFactory;
            _appSettings = fhirStarterSettings;

            ValidationEnabled = ControllerHelper.GetFhirStarterSettingBool(_appSettings, "EnableValidation");
            IsMockupEnabled = ControllerHelper.GetFhirStarterSettingBool(_appSettings, "MockupEnabled");
            _fhirServices = ControllerHelper.GetFhirServices(serviceProvider);
        }
    }
}
