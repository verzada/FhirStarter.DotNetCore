using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FhirStarter.STU3.Detonator.Core.Interface;
using FhirStarter.STU3.Detonator.Core.SparkEngine.Extensions;
using FhirStarter.STU3.Instigator.Core.Helper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace FhirStarter.STU3.Instigator.Core.Controllers
{
    [Route("fhir"), EnableCors]
    public class FhirController : Controller
    {
        private readonly AbstractStructureDefinitionService _abstractStructureDefinitionService;
        private ILogger<IFhirService> _log;

        //public FhirController(AbstractStructureDefinitionService abstractStructureDefinitionService)
        //{
        //    _abstractStructureDefinitionService = abstractStructureDefinitionService;
        //}
        public FhirController(ILogger<IFhirService> loggerFactory)
        {
            _log = loggerFactory;
        }

        #region structure definitions

        [HttpGet, Route("StructureDefinition/{nspace}/{id}")]
        public HttpResponseMessage GetStructureDefinition(string nspace, string id)
        {
            var structureDefinition = Load(false, id, nspace);
            if (structureDefinition == null)
                throw new FhirOperationException($"{nameof(StructureDefinition)} for {nspace}/{id} not found",
                    HttpStatusCode.InternalServerError);
            return SendResponse(structureDefinition);
        }

        [HttpGet, Route("StructureDefinition/{id}")]
        public HttpResponseMessage GetStructureDefinition(string id)
        {
            var structureDefinition = Load(false, id);
            if (structureDefinition == null)
                throw new FhirOperationException($"{nameof(StructureDefinition)} for {id} not found",
                    HttpStatusCode.InternalServerError);
            return SendResponse(structureDefinition);
        }

        private StructureDefinition Load(bool excactMatch, string id, string nspace = null)
        {
            string lookup;
            if (string.IsNullOrEmpty(nspace))
            {
                lookup = id;
            }
            else
            {
                lookup = nspace + "/" + id;
            }

            var structureDefinitions = _abstractStructureDefinitionService.GetStructureDefinitions();
            return excactMatch
                ? structureDefinitions.FirstOrDefault(definition => definition.Type.Equals(lookup))
                : structureDefinitions.FirstOrDefault(definition => definition.Url.EndsWith(lookup));

        }

        #endregion structure definitions

        [HttpGet, Route("fhir/{type}"),FormatFilter]
        public ActionResult Read(string type)
        {
            _log.LogInformation("This works!");

            var searchParams = Request.GetSearchParams();
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);

            var result = service.Read(searchParams);
            //var services = HttpContext;
            //if (services != null)
            //{
            //    var lala = GetFhirService(type, services.RequestServices);
            //    var request = services.RequestServices.GetService<IEnumerable<IFhirService>>().FirstOrDefault(p => p.GetServiceResourceReference().Equals(type));
            //}

            //var test = searchParams;

            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //var parameters = Request.GetSearchParams();
            //if (!(parameters.Parameters.Count > 0)) return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            //var results = service.Read(parameters);
           // return SendResponse(result);
            //return result;
            //var returnObject = (object) result;
            return Ok(result);
        }

        [HttpGet, Route("{type}/{id}"), Route("{type}/identifier/{id}")]
        public async Task<ActionResult<Base>> Read(string type, string id)
        {
            _log.LogInformation("Request with string id: " + id);
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            var result = service.Read(id);
            _log.LogInformation("Result returned");
            //return Ok(result);

            //var okObject = new OkObjectResult(new {message = "200 OK", result});
            //return okObject;
            return result;
        }

        [HttpGet, Route("")]
        // ReSharper disable once InconsistentNaming
        public HttpResponseMessage Query(string _query)
     {
         throw new NotImplementedException();
     }

        [HttpPost, Route("{type}")]
        public HttpResponseMessage Create(string type, Resource resource)
        {
           throw new NotImplementedException();
        }

        [HttpPut, Route("{type}/{id}")]
        public HttpResponseMessage Update(string type, string id, Resource resource)
        {
           throw new NotImplementedException();
        }

        [HttpDelete, Route("{type}/{id}")]
        public HttpResponseMessage Delete(string type, string id)
        {
           
            throw new NotImplementedException();
        }

        [HttpPatch, Route("{type}/{id}")]
        public HttpResponseMessage Patch(string type, string id, Resource resource)
        {
           
            throw new NotImplementedException();
           }


        [HttpGet, Route("metadata")]
        public HttpResponseMessage MetaData()
        {
            throw new NotImplementedException();

            var headers = Request.Headers;
            var accept = headers;
           var returnJson = accept[HeaderNames.ContentType] == "application/json";
            var whatis = accept[HeaderNames.ContentType];
            var returnXml = accept[HeaderNames.ContentType] == "application/xml";
            //var returnJson = accept.Any(x => x.Contains(FhirMediaType.HeaderTypeJson));

            //StringContent httpContent;
            //var metaData = _handler.CreateMetadata(_fhirServices, _abstractStructureDefinitionService, Request.RequestUri.AbsoluteUri);
            //if (!returnJson)
            //{
            //    var xmlSerializer = new FhirXmlSerializer();
            //    var xml = xmlSerializer.SerializeToString(metaData);
            //    httpContent =
            //        new StringContent(xml, Encoding.UTF8,
            //            "application/xml");
            //}
            //else
            //{
            //    var jsonSerializer = new FhirJsonSerializer();
            //    var json = jsonSerializer.SerializeToString(metaData);
            //    httpContent =
            //        new StringContent(json, Encoding.UTF8,
            //            "application/json");
            //}
            //var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = httpContent };
            //return response;
            return null;
        }


        #region private methods

        private HttpResponseMessage SendResponse(Base resource)
        {
            var searchParams = Request.GetSearchParams();
            var format = searchParams.Parameters.FirstOrDefault(p => p.Item1.Contains("_format"));

            var headers = Request.Headers;
            //var accept = headers.;

            //var returnJson = ReturnJson(accept);
            //if (!(resource is OperationOutcome))
            //{
            //    resource = ValidateResource((Resource) resource, false);
            //}

            //StringContent httpContent;
            //if (!returnJson)
            //{
            //    var xmlSerializer = new FhirXmlSerializer();
            //    httpContent =
            //        GetHttpContent(xmlSerializer.SerializeToString(resource), FhirMediaType.XmlResource);
            //}
            //else
            //{
            //    var jsonSerializer = new FhirJsonSerializer();
            //    httpContent =
            //        GetHttpContent(jsonSerializer.SerializeToString(resource), FhirMediaType.JsonResource);
            //}
            //var response = new HttpResponseMessage(HttpStatusCode.OK) {
            //    Content = httpContent
            //};
            //return response;
            return null;
        }

        private Base ValidateResource(Resource resource, bool isInput)
        {
            //lock (ValidationLock)
            //{
            //    if (_profileValidator == null) return resource;
            //    if (resource is OperationOutcome) return resource;
            //    {
            //        var resourceName = resource.TypeName;
            //        var structureDefinition = Load(true, resourceName);
            //        if (structureDefinition != null)
            //        {
            //            var found = resource.Meta != null && resource.Meta.ProfileElement.Count == 1 &&
            //                        resource.Meta.ProfileElement[0].Value.Equals(structureDefinition.Url);
            //            if (!found)
            //            {
            //                var message = $"Profile for {resourceName} must be set to: {structureDefinition.Url}";
            //                if (isInput)
            //                {
            //                    throw new ValidateInputException(message);
            //                }

            //                throw new ValidateOutputException(message);

            //            }
            //        }

            //    }
            //    var validationResult = _profileValidator.Validate(resource, true, false);
            //    if (validationResult.Issue.Count > 0)
            //    {
            //        resource = validationResult;
            //    }
            //    return resource;
            //}            
            return null;
        }
        private static bool ReturnJson(HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> accept)
        {
            var jsonHeaders = ContentType.JSON_CONTENT_HEADERS;
            var returnJson = false;
            foreach (var x in accept)
            {
                if (jsonHeaders.Any(y => x.MediaType.Contains(y)))
                {
                    returnJson = true;
                }
            }
            return returnJson;
        }
        #endregion private methods
    }

    

}
