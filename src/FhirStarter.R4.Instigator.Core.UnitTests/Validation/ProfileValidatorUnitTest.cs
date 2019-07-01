using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using FhirStarter.R4.Detonator.Core.Interface;
using FhirStarter.R4.Instigator.Core.Validation;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;
using Microsoft.Extensions.Logging;
using No.Helsenord.Common.Converter.Utils;
using NUnit.Framework;
using List = Hl7.Fhir.Model.List;

namespace FhirStarter.R4.Instigator.Core.UnitTests.Validation
{
    [TestFixture]
    internal class ProfileValidatorUnitTest
    {

        private ProfileValidator _profileValidator;

        [OneTimeSetUp]
        public void Setup()
        {
            
            _profileValidator = new ProfileValidator(ValidatorFactory.GetValidator(), GetLogger());
        }

        private static ILogger GetLogger()
        {
            var factory = new LoggerFactory();
            return factory.CreateLogger("MyLogger");
            
        }

        

        [TestCase("12345123451")]
        public void TestValidateStandardPatient(string id)
        {
            var patient = GetSomePatient(id);
            var validated = DoValidation(patient);
            Assert.IsTrue(validated.Issue.Count == 0);
        }

        [TestCase("12345123451")]
        public void TestValidateStandardPatientShouldFail(string id)
        {
            var patient = GetSomePatientWithProfile(id);
            var validated = DoValidation(patient);
            
            Assert.IsTrue(validated.Issue.Count > 0);
        }

        [TestCase("1234512345", "22222222222", "33333333333")]
        public void TestValidateBundleWithPatients(params string[] patients)
        {
            var bundle = new Bundle();
            foreach (var patient in patients)
            {

                var entry = new Bundle.EntryComponent {Resource = GetSomePatient(patient)};
                bundle.Entry.Add(entry);
            }

            bundle.Total = bundle.Entry.Count;
            var validated = DoValidation(bundle);
            var xml = new FhirXmlSerializer().SerializeToDocument(validated);
            Console.WriteLine(xml);
        }

        [TestCase(1000, true, "12345123451")]
        [TestCase(1000, false, "12345123451")]
        public void TestValidation(int tries, bool parallel, string patientId)
        {
            var patient = GetSomePatient(patientId);
            var itemsToRun = new List<DelegateParameters>();
            for (var i = 0; i < tries; i++)
            {
                var parameters = new List<object> {patient};
                Func<Resource, OperationOutcome> func = DoValidation;
                itemsToRun.Add(new DelegateParameters
                {
                    Delegate = func,
                    Parameters = parameters.ToArray()
                });
            }
            DelegateRunner.Run(itemsToRun, parallel);
        }

        private OperationOutcome DoValidation(Resource arg)
        {
            return _profileValidator.Validate(arg);
        }

        private static Patient GetSomePatient(string patientId)
        {
            var date = new FhirDateTime(new DateTimeOffset(DateTime.Now));
            return new Patient
            {
                Meta = new Meta { LastUpdated = date.ToDateTimeOffset(new TimeSpan()) },
                Id = patientId,
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

        private static Patient GetSomePatientWithProfile(string patientId)
        {
            var date = new FhirDateTime(new DateTimeOffset(DateTime.Now));

            return new Patient
            {
                Meta = new Meta { LastUpdated = date.ToDateTimeOffset(new TimeSpan()), Profile = new List<string> { "http://helse-nord.no/FHIR/profiles/Identification.Patient/Patient" } },
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
