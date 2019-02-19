using System.Collections.Generic;

namespace FhirStarter.R4.Instigator.Core.Model
{
    public class FhirStarterSettings
    {
        public List<string> FhirServiceAssemblies { get; set; }
        public bool MockupEnabled { get; set; }
        public bool EnableValidation { get; set; }
        public bool LogRequestWhenError { get; set; }
        public string FhirPublisher { get; set; }
    }
}
