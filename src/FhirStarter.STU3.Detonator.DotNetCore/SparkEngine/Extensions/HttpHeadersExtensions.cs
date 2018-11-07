using System.Net.Http;
using System.Net.Http.Headers;

namespace FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Extensions
{
    public static class HttpHeadersExtensions
    {
        public static void Replace(this HttpHeaders headers, string header, string value)
        {
            //if (headers.Exists(header)) 
            headers.Remove(header);
            headers.Add(header, value);
        }

        public static string GetParameter(this HttpRequestMessage request, string key)
        {
            //foreach (var param in request.GetQueryNameValuePairs())
            foreach (var param in request.Properties)
            {
                if (param.Key == key) return param.Value.ToString();
            }
            return null;
        }
    }
}

