# Step 2 Setting up a Resource Service

## Some quick pointers in order to successfully create a FHIR Resource service

FhirStarter is built around the [.NET API for HL7 FHIR](https://github.com/FirelyTeam/fhir-net-api) and its conventions. 
A [Resource](https://www.hl7.org/fhir/resource.html) service in FhirStarter is based on classes that inherits the [Base](https://github.com/FirelyTeam/fhir-net-api/blob/develop-stu3/src/Hl7.Fhir.Core/Model/Base.cs) model.

Many endpoints (Create, Read, Update, etc .. ) looks into the [SearchParam](https://github.com/FirelyTeam/fhir-net-api/blob/develop-stu3/src/Hl7.Fhir.Core/Rest/SearchParams.cs) object to figure out what is being queried for.
The SearchParams object will only accept valid property names in the request url.

There are three possible outcomes from a FHIR Service:
- A xml or json result which is either a Resource or a [Bundle](https://www.hl7.org/fhir/bundle.html)
- An [OperationOutcome](https://www.hl7.org/fhir/operationoutcome.html) if an exception occurs.
- A validation report in form of a OperationOutcome if validation is enabled and the input or output doesn't match the profile.
  - Input is typically create, patch or update
  - Output is the response returned to the client from the server


You'll need to handle and test for these three scenarios.

## The steps in setting up a Resource service 

### The IFhirService

To create a new Resource service with the IFhirService interface, just create a new file in your web application. Initially it can typically look something like this:

```c#
using System;
using System.Collections.Generic;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Detonator.Core.SparkEngine.Core;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatientAdministration.Person.R4.Services
{
    public class ResourceService:IFhirService
    {
        public string GetServiceResourceReference()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public OperationDefinition GetOperationDefinition(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public string GetStructureDefinitionNameForResourceProfile()
        {
            throw new NotImplementedException();
        }
    }
}
```

#### GetServiceResourceReference

Is used to define the FHIR Resource exposed in the service. There can only be one FHIR resource per service (see our example of a FHIR service, but many services in the same Twisted project).
Normally you'll just need to add a nameof and the name of the Resource which is used in the Url to access the service

```c#
        public string GetServiceResourceReference()
        {
            return nameof(Patient);
        }
```

In the case above the service is accessed like this:

 ``` http://<domainurl>/fhir/Patient ```

If you have a service which represents a custom query, you can add the name of the custom query service instead of a service. In order to query the service, you'll need to use the path

``` /fhir?_query=<customQueryService>&param1=..&param2=... ``` 

If you are using the FHIR client (from the R4 api), you'll have to add the name to the Query attribute when you're adding SearchParams.

```c#
    var searchParams = new SearchParams();
    searchParams.Add("name", "Danser");
    searchParams.Query = "<nameOfCustomService>";
    var result = client.Search(searchParams);
```
Note the above code is from DSTU2, but (hopefully) you get the idea.

#### GetRestDefinition

To make it easier for your consumers of the FHIR service to understand how to query the service, you should create a [CapabilityStatement description](https://www.hl7.org/fhir/capabilitystatement.html).

One way of exposing the metadata is through a custom xml which is parsed, which may be easier to maintain. It is possible to write [ImplementationGuide](https://www.hl7.org/fhir/implementationguide.html)s in [Forge](https://fire.ly/products/forge/) which can be maintained along side the profiles. If you have an xml, you can create something like this that's specific for each service:

```c#
        public CapabilityStatement.RestComponent GetRestDefinition()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();
            foreach (var name in names)
            {
                if (!name.EndsWith("ExampleServiceRest.xml")) continue;
                using (var stream = assembly.GetManifestResourceStream(name))
                {
                    var xDocument = XDocument.Load(stream);
                    var parser = new FhirXmlParser();
                    var item =
                        parser.Parse<CapabilityStatement>(xDocument.ToString());
                    return item.Rest[0];
                }
            }
            throw new InvalidDataException();
        }
```

Otherwise you can create programatically the REST defintion for each Resource service:

```c#
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
```

**Note** the example above is missing defining the valid [SearchParameters](https://www.hl7.org/fhir/searchparameter.html) for the particular Resource service.

To see how the CapabilityStatement is put together, see take a look at the [CapabilityStatementBuilder](https://github.com/verzada/FhirStarter.DotNetCore/blob/master/src/FhirStarter.R4.Detonator.Core/SparkEngine/Service/FhirServiceExtensions/CapabilityStatementBuilder.cs).


#### GetOperationDefinition

If you have operations, typically used when you have a custom query search, you add a [OperationDefinition](https://www.hl7.org/fhir/operationdefinition.html). The OperationDefintions is linked in the metadata of the FHIR service. The links should be valid by default and not give an error upon request.

#### CRUD - Create Read Update Delete

##### Create
The method receives a key and a FHIR resource. The key can be used to identify the resource your backend, however you can have identifiers in the resource as well which can make the key dispensable. But there are cases where the identifier in the resource does not correspond with the backend.

```c# 
        public Base Create(IKey key, Resource resource)
        {            
            var request = (CommunicationRequest)resource;           
            var result = _service.Post(request);       
            return result;
        }
``` 

Above is code from STU3, the concept is still the same; A Resource is received, which can be converted to the correct Resource type or processed by a different underlying service.

##### Read
There are two Read methods, one which takes a identifier, the other reads the FHIR query. The identifier can be anything as long as it is valid in your service and backend.

For example:
- The identifier is a string of a valid SSN 
- The identifier is a string of a valid GUID in the database 
- The identier is a property of the resource, Patient.identifier which is a valid internal id in the database 
etc  

###### Example of Read with SearchParams
```c# 
        public Base Read(SearchParams searchParams)
        {
            var person = new Hl7.Fhir.Model.Person();
            person.Name = new List<HumanName>();
            var humanName = new HumanName();
            humanName.Family = "Smith";
            person.Name.Add(humanName);

            return person;
        }
``` 

###### Example of Read with id
```c#
        public Base Read(string id)
        {
            var soapHl7Request = internalPersonService.GeneratePatientHl7Request(id);
            var soapResponse = internalPersonService.SendSoapRequest(soapHl7Request);
            if (soapResponse != null)
            {
                var hl7PersonResponse = RequestHelper.TransformSoapHl7PatientResponseToHl7Patient(soapResponse);
                if (ValidateHl7PersonResponse(hl7PersonResponse))
                {
                    var personDoc = RequestHelper.TransformHl7PatientToFhirPerson(hl7PersonResponse);
                    if (personDoc.Root != null && personDoc.Root.Elements().Any())
                    {
                        var parser = new FhirXmlParser();
                        var person = parser.Parse<Hl7.Fhir.Model.Person>(personDoc.ToString());
                        return person;
                    }
                }
            }

            return new Hl7.Fhir.Model.Person();
        }
```


##### Update
The update method is similar to the Create method, however instead of adding a new resource the update method will let you handle updates of a given resource.

##### Delete
Here you have only one parameter and that's the identifier of your FHIR resource.

##### Patch
Unlike Update, Patch will let you update part of a resource instead of the whole resource. F.ex if you want to update a field in a resource or your datasource.

##### Key - todo check this part out 
-- TODO


#### StructureDefinition

##### GetStructureDefinitionNameForResourceProfile

You can add your custom StructureDefinition to the service, by adding the name of the StructureDefinition (see the [Getting started with Validation](../Validation/GettingStartedWithValidation.md) for a quick howto).
