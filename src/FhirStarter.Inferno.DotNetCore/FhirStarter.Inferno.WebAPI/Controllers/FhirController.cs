using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using FhirStarter.Bonfire.DotNetCore.Interface;
using FhirStarter.Inferno.WebAPI.Extensions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FhirStarter.Inferno.WebAPI.Controllers
{
    [Route("fhir"), EnableCors]
    public class FhirController : Controller
    {
        private readonly AbstractStructureDefinitionService _abstractStructureDefinitionService;

        //public FhirController(AbstractStructureDefinitionService abstractStructureDefinitionService)
        //{
        //    _abstractStructureDefinitionService = abstractStructureDefinitionService;
        //}
        public FhirController(){}

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

        [HttpGet, Route("{type}")]
        public void Read(string type)
        {
            var searchParams = Request.GetSearchParams();

            //var test = searchParams;

            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //var parameters = Request.GetSearchParams();
            //if (!(parameters.Parameters.Count > 0)) return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            //var results = service.Read(parameters);
            //return SendResponse(results);

        }

          [HttpGet, Route("{type}/{id}"), Route("{type}/identifier/{id}")]
        public HttpResponseMessage Read(string type, string id)
        {
            //if (type.Equals(nameof(OperationDefinition)))
            //{
            //    var operationDefinitions = _handler.GetOperationDefinitions(id, _fhirServices);
            //    return SendResponse(operationDefinitions);
            //}

            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //var result = service.Read(id);

            //return SendResponse(result);
            return SendResponse(new AllergyIntolerance());
        }
       
     [HttpGet, Route("")]
        // ReSharper disable once InconsistentNaming
        public HttpResponseMessage Query(string _query)
        {
            //var searchParams = Request.GetSearchParams();
            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, searchParams.Query);
            //var result = service.Read(searchParams);

            //return SendResponse(result);

            return SendResponse(new AllergyIntolerance());
        }

        [HttpPost, Route("{type}")]
        public HttpResponseMessage Create(string type, Resource resource)
        {
            var xmlSerializer = new FhirXmlSerializer();            
            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);            
            //resource = (Resource) ValidateResource(resource, true);
            //return resource is OperationOutcome ? SendResponse(resource) : _handler.ResourceCreate(type, resource, service);

            return SendResponse(new AllergyIntolerance());
        }

        [HttpPut, Route("{type}/{id}")]
        public HttpResponseMessage Update(string type, string id, Resource resource)
        {
            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //return _handler.ResourceUpdate(type, id, resource, service);

            return SendResponse(new AllergyIntolerance());
        }

        [HttpDelete, Route("{type}/{id}")]
        public HttpResponseMessage Delete(string type, string id)
        {
            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //return _handler.ResourceDelete(type, Key.Create(type, id), service);

            return SendResponse(new AllergyIntolerance());
        }

        [HttpPatch, Route("{type}/{id}")]
        public HttpResponseMessage Patch(string type, string id, Resource resource)
        {
            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //return _handler.ResourcePatch(type, Key.Create(type, id), resource, service);
           
            return SendResponse(new AllergyIntolerance());
           }


        [HttpGet, Route("metadata")]
        public HttpResponseMessage MetaData()
        {
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
            //var headers = Request.Headers;
            //var accept = headers.Accept;

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
