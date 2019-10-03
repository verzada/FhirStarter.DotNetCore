using System;
using System.Collections.Generic;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Instigator.Core.Helper;
using FhirStarter.R4.Instigator.Core.Validation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FhirStarter.R4.Instigator.Core.Controllers
{
    [Route("fhir"), EnableCors]
    public partial class FhirController
    {

        private ILogger<IFhirService> _log;
        private readonly IConfigurationRoot _appSettings;
        private readonly IEnumerable<IFhirService> _fhirServices;
        private readonly IProfileValidator _profileValidator;

        private readonly bool _validationEnabled;

        private bool _isMockupEnabled;
        //private AcceptHeaderAttribute _acceptHeaderAttributes;

        public FhirController(ILogger<IFhirService> loggerFactory, IConfigurationRoot fhirStarterSettings,
            IServiceProvider serviceProvider, IProfileValidator profileValidator)
        {
            _log = loggerFactory;
            _appSettings = fhirStarterSettings;

            _validationEnabled = ControllerHelper.GetFhirStarterSettingBool(_appSettings, "EnableValidation");
            _isMockupEnabled = ControllerHelper.GetFhirStarterSettingBool(_appSettings, "MockupEnabled");
            _fhirServices = ControllerHelper.GetFhirServices(serviceProvider);
            _profileValidator = profileValidator;
        }
    }
}
