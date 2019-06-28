using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using FhirStarter.R4.Detonator.Core.Helper;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Detonator.Core.SparkEngine.Core;
using FhirStarter.R4.Instigator.Core.Helper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FhirStarter.R4.Twisted.Core.Services
{
    public class PatientService:IFhirService
    {
        public PatientService()
        {
#pragma warning disable 219
            int i = 0;
#pragma warning restore 219
        }

        public string GetServiceResourceReference()
        {
            return nameof(Patient);
        }

        public Base Create(IKey key, Resource resource)
        {
            throw new NotImplementedException();
        }

        public Base Read(SearchParams searchParams)
        {
            var parameters = searchParams.Parameters;
            var xmlSerializer = new FhirXmlSerializer();
            foreach (var parameter in parameters)
            {
                if (parameter.Item1.ToLower().Contains("log") && parameter.Item2.ToLower().Contains("normal"))
                {
                    throw new ArgumentException("Using " + nameof(SearchParams) +
                                                " in Read(SearchParams searchParams) should throw an exception which is put into an OperationOutcomes issues");
                }
                if (parameter.Item1.Contains("log") && parameter.Item2.Contains(nameof(OperationOutcome).ToLower()))
                {
                    var operationOutcome = new OperationOutcome{Issue = new List<OperationOutcome.IssueComponent>()};
                    var issue = new OperationOutcome.IssueComponent
                    {
                        Severity = OperationOutcome.IssueSeverity.Information,
                        Code = OperationOutcome.IssueType.Incomplete,
                        Details = new CodeableConcept("SomeExampleException", typeof(FhirOperationException).ToString(),
                            "Something expected happened and needs to be handled with more detail.")
                    };
                    operationOutcome.Issue.Add(issue);
                    //var errorMessage = fh
                    
                    //var serialized = FhirSerializer.SerializeResourceToXml(operationOutcome);
                    var serialized = xmlSerializer.SerializeToString(operationOutcome);
                    throw new ArgumentException(serialized);
                }
            }
            throw new ArgumentException("Generic error");
        }

        public Base Read(string id)
        {
            throw new ArgumentException($"Please throw an {nameof(ArgumentException)} and create an {nameof(OperationOutcome)} of it");
            return MockPatient();
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

        public OperationDefinition GetOperationDefinition(HttpRequest request)
        {
            var current = HtmlHelper.GetDisplayUrlFromRequest(request);
             var definition = new OperationDefinition
            {
                Url = UrlHandler.GetUrlForOperationDefinition(current, string.Empty, nameof(Patient)),
                Name = GetServiceResourceReference(),
                Status = PublicationStatus.Active,
                Kind = OperationDefinition.OperationKind.Query,
                Experimental = false,
                Code = GetServiceResourceReference(),
                Description = new Markdown("Search parameters for the test query service"),
                System = true,
                Instance = false,
                Parameter =
                    new List<OperationDefinition.ParameterComponent>
                    {
                        new OperationDefinition.ParameterComponent
                        {
                            Name = "Name",
                            Use = OperationParameterUse.In,
                            Type = FHIRAllTypes.String,
                            Min = 0,
                            Max = "1"
                        },
                        new OperationDefinition.ParameterComponent
                        {
                            Name = "Name:contains",
                            Use = OperationParameterUse.In,
                            Type = FHIRAllTypes.String,
                            Min = 0,
                            Max = "1"
                        },
                        new OperationDefinition.ParameterComponent
                        {
                            Name = "Name:exact",
                            Use = OperationParameterUse.In,
                            Type = FHIRAllTypes.String,
                            Min = 0,
                            Max = "1"
                        },
                        new OperationDefinition.ParameterComponent
                        {
                            Name = "Identifier",
                            Use = OperationParameterUse.In,
                            Type = FHIRAllTypes.String,
                            Min = 0,
                            Max = "1",
                            Documentation = "Query against the following: REKVIRENTKODE, HPNR or HER-ID"
                        },
                        new OperationDefinition.ParameterComponent
                        {
                            Name = "_lastupdated",
                            Use = OperationParameterUse.In,
                            Type = FHIRAllTypes.String,
                            Min = 0,
                            Max = "2",
                            Documentation =
                                "Equals" + " -- Note that the date format is yyyy-MM-ddTHH:mm:ss --"
                        }
                    }
            };
            return definition;
        }

        public ICollection<string> GetStructureDefinitionNames()
        {
            return new List<string> { GetServiceResourceReference() };
        }

        private static Base MockPatient()
        {
            var date = new FhirDateTime(DateTime.Now);

            return new Patient
            {
                Meta = new Meta { LastUpdated = date.ToDateTimeOffset(), Profile = new List<string> { "http://helse-nord.no/FHIR/profiles/Identification.Patient/Patient" } },
                Id = "12345678901",
                Active = true,
                Name =
                    new List<HumanName>
                    {
                        new HumanName{Family = "Normann", Given = new List<string>{"Ola"}}
                    },
                Telecom =
                    new List<ContactPoint>
                    {
                        new ContactPoint {System = ContactPoint.ContactPointSystem.Phone, Value = "123467890"}
                    },
                Gender = AdministrativeGender.Male,
                BirthDate = "2000-01-01"
                
            };
        }
    }
}
