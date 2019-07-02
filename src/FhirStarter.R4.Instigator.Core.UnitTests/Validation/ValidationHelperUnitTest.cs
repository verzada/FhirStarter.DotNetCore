using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FhirStarter.R4.Instigator.Core.Validation;
using NUnit.Framework;

namespace FhirStarter.R4.Instigator.Core.UnitTests.Validation
{
    [TestFixture]
    internal class ValidationHelperUnitTest
    {

        [Test]
        public void TestGetStructureDefinitons()
        {
            var result = ValidationHelper.GetStructureDefinitions();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}
