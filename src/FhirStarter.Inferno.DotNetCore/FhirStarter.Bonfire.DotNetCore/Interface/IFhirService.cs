using System.Collections.Generic;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Http;

namespace FhirStarter.Bonfire.DotNetCore.Interface
{
    /// <summary>
    /// The interface used by the FhirStarter Inferno server to expose the fhir service. 
    /// It is not meant to handle internal services
    /// </summary>
    public interface IFhirService:IFhirBaseService
    {
        CapabilityStatement.RestComponent GetRestDefinition();
        OperationDefinition GetOperationDefinition(HttpRequest request);
        ICollection<string> GetStructureDefinitionNames();

    }
}
