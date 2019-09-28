using System;
using System.Reflection;
using System.Xml.Linq;
using FhirStarter.R4.Instigator.Core.Validation;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace FhirStarter.R4.Instigator.Core.UnitTests.Validation
{
    [TestFixture]
    internal class ValidateNationalOrganizationIntegrationTest
    {

        private ProfileValidator _profileValidator;

        [OneTimeSetUp]
        public void Setup()
        {
             _profileValidator = new ProfileValidator(ValidationHelper.GetValidator(), GetLogger());
        }

        private static ILogger GetLogger()
        {
            var factory = new LoggerFactory();
            return factory.CreateLogger("MyLogger");
        }

        //[TestCase("OrganizationForHelseViken.xml")]
        //public void ValidateOrganization(string path)
        //{
        //    using (var stream = AssemblyHelper.GetStream(path, Assembly.GetExecutingAssembly()))
        //    {
        //        var xDocument = XDocument.Load(stream);
        //        var organization = new FhirXmlParser().Parse<Organization>(xDocument.ToString());
        //        var valid = _profileValidator.Validate(organization);
        //        var serialized = new FhirXmlSerializer().SerializeToDocument(valid);
        //        Console.WriteLine(serialized);
        //        Assert.IsTrue(valid.Issue.Count == 0);
        //    }
        //}
    }
}
