using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Detonator.Core.SparkEngine.Core;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FhirStarter.R4.Twisted.Core.Services
{
    public class ResourceService:IFhirService
    {
        public string GetServiceResourceReference()
        {
            return nameof(Resource);
        }

        public Base Create(IKey key, Resource resource)
        {
            throw new NotImplementedException();
        }

        public Base Read(SearchParams searchParams)
        {
            throw new NotImplementedException();
        }

        public Base Read(string id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Update(IKey key, Resource resource)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(IKey key)
        {
            throw new NotImplementedException();
        }

        public ActionResult Patch(IKey key, Resource resource)
        {
            throw new NotImplementedException();
        }

        public CapabilityStatement.RestComponent GetRestDefinition()
        {
            var resourceComponent = new CapabilityStatement.ResourceComponent
            {
                Type = ResourceType.Resource,
                Profile = "url for structure defintion",
                Interaction = new List<CapabilityStatement.ResourceInteractionComponent>
                {
                    new CapabilityStatement.ResourceInteractionComponent
                    {
                        Code = CapabilityStatement.TypeRestfulInteraction.Create
                    },
                    new CapabilityStatement.ResourceInteractionComponent
                    {
                        Code = CapabilityStatement.TypeRestfulInteraction.Read
                    },
                    new CapabilityStatement.ResourceInteractionComponent
                    {
                        Code = CapabilityStatement.TypeRestfulInteraction.Update
                    },
                    new CapabilityStatement.ResourceInteractionComponent
                    {
                        Code = CapabilityStatement.TypeRestfulInteraction.Delete
                    }
                },
                Versioning = CapabilityStatement.ResourceVersionPolicy.NoVersion
            };
            var restComponent = new CapabilityStatement.RestComponent();
            restComponent.Resource.Add(resourceComponent);
            return restComponent;
        }

        public OperationDefinition GetOperationDefinition(HttpRequest request)
        {
           return new OperationDefinition();
        }

        public ICollection<string> GetStructureDefinitionNames()
        {
            return new List<string>();
        }
    }
}
