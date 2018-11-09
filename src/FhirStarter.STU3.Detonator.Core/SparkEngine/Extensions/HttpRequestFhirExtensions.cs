using System.Net.Http;
using FhirStarter.STU3.Detonator.Core.SparkEngine.Core;
using FhirStarter.STU3.Detonator.Core.SparkEngine.Service;
using Hl7.Fhir.Rest;

namespace FhirStarter.STU3.Detonator.Core.SparkEngine.Extensions
{
    public static class HttpRequestFhirExtensions
    {
        public static Entry GetEntry(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(Const.RESOURCE_ENTRY))
                return request.Properties[Const.RESOURCE_ENTRY] as Entry;
            return null;
        }

        public static SummaryType RequestSummary(this HttpRequestMessage request)
        {
            return request.GetParameter("_summary") == "true" ? SummaryType.True : SummaryType.False;
        }
    }
}
