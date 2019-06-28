using System;
using System.Net.Http;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Detonator.Core.SparkEngine.Core;
using FhirStarter.R4.Detonator.Core.SparkEngine.Extensions;
using FhirStarter.R4.Instigator.Core.Helper;
using FhirStarter.R4.Instigator.Core.Model;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FhirStarter.R4.Instigator.Core.Controllers
{
    public partial class FhirController : Controller
    {
        #region HelperMethods
        private ActionResult HandleServiceReadWithSearchParams(string type)
        {
            var searchParams = Request.GetSearchParams();
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            if (service != null)
            {
                var result = service.Read(searchParams);
                return HandleResult(result);
            }
            throw new ArgumentException($"The resource {type} service does not exist!");
        }

        private ActionResult HandleServiceRead(string type, string id)
        {
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            if (service != null)
            {
                var result = service.Read(id);
                return HandleResult(result);
            }
            throw new ArgumentException($"The resource {type} service does not exist!");
        }

        private ActionResult HandleResult(Base result)
        {
            if (result is OperationOutcome)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        private ActionResult ResourceCreate(string type, Resource resource, IFhirBaseService service)
        {
            if (service != null && !string.IsNullOrEmpty(type) && resource != null)
            {
                var key = Key.Create(type);
                var result = service.Create(key, resource);
                if (result != null)
                {
                    if (result is OperationOutcome)
                    {
                        return BadRequest(result);
                    }
                    return Ok(result);
                }
            }
            return BadRequest($"Service for resource {nameof(resource)} must exist.");
        }

        public ActionResult ResourceUpdate(string type, string id, Resource resource, IFhirBaseService service)
        {
            if (service == null || string.IsNullOrEmpty(type) || resource == null || string.IsNullOrEmpty(id))
                throw new ArgumentException("Service is null, cannot update resource of type " + type);
            var key = Key.Create(type, id);
            var result = service.Update(key, resource);
            if (result != null)
                return Ok(result);
            return BadRequest($"Service is null, cannot update resource of {type}");
        }

        public ActionResult ResourceDelete(string type, Key key, IFhirBaseService service)
        {
            if (service != null)
            {
                var result = service.Delete(key);
                return result;
            }
           return BadRequest($"Service is null, cannot update resource of {type}");
        }

        public ActionResult ResourcePatch(string type, IKey key, Resource resource, IFhirBaseService service)
        {
            if (service != null)
            {
                var result = service.Patch(key, resource);
                return result;
            }
            return BadRequest($"Service is null, cannot update resource of {type}");
        }

        #endregion HelperMethods

        [HttpGet, Route("{type}/{id}"), Route("{type}/identifier/{id}")]
        public ActionResult Read(string type, string id)
        {
            return HandleServiceRead(type, id);
        }

        [HttpGet, Route("fhir/{type}"), FormatFilter]
        public ActionResult Read(string type)
        {
            return HandleServiceReadWithSearchParams(type);
        }

        [HttpGet, Route("")]
        // ReSharper disable once InconsistentNaming
        public ActionResult Query(string type, string _query)
        {
            return HandleServiceReadWithSearchParams(type);
        }

        // return 201 when created
        // return 202 when takes too long
        [HttpPost, Route("{type}")]
        public ActionResult Create(string type, Resource resource)
        {
            if (ValidationEnabled)
            {
                resource = (Resource) ValidateResource(resource, true);
            }
            if (resource is OperationOutcome)
            {
                return BadRequest(resource);
            }
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            return ResourceCreate(type, resource, service);
        }

        // return 201 when created
        // return 202 when takes too long
        [HttpPut, Route("{type}/{id}")]
        public ActionResult Update(string type, string id, Resource resource)
        {
            if (ValidationEnabled)
            {
                resource = (Resource) ValidateResource(resource, true);
            }
            if (resource is OperationOutcome)
            {
                return BadRequest(resource);
            }
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            return ResourceUpdate(type, id, resource, service);
        }

        // return 201 when created
        // return 202 when takes too long
        [HttpDelete, Route("{type}/{id}")]
        public ActionResult Delete(string type, string id)
        {
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            if (service != null)
            {
                var result = ResourceDelete(type, Key.Create(id), service);
                return result;
            }
            return BadRequest($"Service is null, cannot delete resource of {type} and id {id}");
        }

        // return 201 when created
        // return 202 when takes too long
        [HttpPatch, Route("{type}/{id}")]
        public ActionResult Patch(string type, string id, Resource resource)
        {
            var service = ControllerHelper.GetFhirService(type, HttpContext.RequestServices);
            if (service != null)
            {
                var result = ResourcePatch(type, Key.Create(id), resource, service);
                return result;
            }
            return BadRequest($"Service is null, cannot delete resource of {type} and id {id}");
        }

        [HttpGet, Route("metadata")]
        public ActionResult MetaData()
        {
            var httpRequest = HttpContext.Request;

            var metaData = ControllerHelper.CreateMetaData(_fhirServices, _abstractStructureDefinitionService, _appSettings, httpRequest);
            return Ok(metaData);
        }
    }
}
