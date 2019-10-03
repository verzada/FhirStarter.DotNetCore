using Hl7.Fhir.Model;

namespace FhirStarter.R4.Instigator.Core.Validation
{

    public interface IProfileValidator
    {
        OperationOutcome Validate(Resource resource, bool onlyErrors = true, bool threadedValidation = true);
    }
}
