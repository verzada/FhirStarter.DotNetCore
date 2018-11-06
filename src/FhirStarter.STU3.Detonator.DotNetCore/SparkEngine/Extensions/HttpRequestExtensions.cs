using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Hl7.Fhir.Rest;

namespace FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Extensions
{
    public static class HttpRequestExtensions
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

        private static List<Tuple<string, string>> TupledParameters(this HttpRequestMessage request)
        {
            var list = new List<Tuple<string, string>>();

         //   var query = request.GetQueryNameValuePairs();
            var query = request.Properties;
            foreach (var pair in query)
            {
                list.Add(new Tuple<string, string>(pair.Key, pair.Value.ToString()));
            }
            return list;
        }

        public static SearchParams GetSearchParams(this HttpRequestMessage request)
        {
            var parameters = request.TupledParameters().Where(tp => tp.Item1 != "_format");
            var searchCommand = SearchParams.FromUriParamList(parameters);
            return searchCommand;
        }
    }
}

