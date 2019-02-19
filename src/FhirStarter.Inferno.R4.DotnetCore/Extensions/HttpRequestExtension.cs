

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace FhirStarter.Inferno.STU3.DotnetCore.Extensions
{
    public static class HttpRequestExtension
    {
        public static void Replace(this HttpHeaders headers, string header, string value)
        {
            throw new NotImplementedException();
        }

        private static List<Tuple<string, string>> TupledParameters(this HttpRequest request)
        {
            var list = new List<Tuple<string, string>>();
            //var query = request.get
            var query = request.Query;
            foreach (var pair in query)
            {
                list.Add(new Tuple<string, string>(pair.Key,pair.Value));
            }

            return list;
        }

        public static SearchParams GetSearchParams(this HttpRequest request)
        {
            var parameters = request.TupledParameters().Where(tp => tp.Item1 != "_format");
            var searchCommand = SearchParams.FromUriParamList(parameters);
            return searchCommand;
        }
    }
}
