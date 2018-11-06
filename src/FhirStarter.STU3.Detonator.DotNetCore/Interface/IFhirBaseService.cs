using System.Net.Http;
using FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Core;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirStarter.STU3.Detonator.DotNetCore.Interface
{
    public interface IFhirBaseService
    {
        // The name of the Resource you can query (earlier called GetAlias)
        string GetServiceResourceReference();
        HttpResponseMessage Create(IKey key, Resource resource);
        Base Read(SearchParams searchParams);
        Base Read(string id);
        HttpResponseMessage Update(IKey key, Resource resource);
        HttpResponseMessage Delete(IKey key);
        HttpResponseMessage Patch(IKey key, Resource resource);
    }
}
