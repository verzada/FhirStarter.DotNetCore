using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using FhirStarter.Bonfire.DotNetCore.Interface;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Validation;
using Microsoft.Extensions.Logging;

namespace FhirStarter.Inferno.WebAPI.Config
{
      public class ProfileValidator
    {
        private ILogger<IFhirService> _log;
        private static Validator _validator;
        
        public ProfileValidator(Validator validator, ILogger<IFhirService> logger)
        {
            if (_validator == null)
            {
                _validator = validator;
            };
            _log = logger;
        }

        public OperationOutcome Validate(Resource resource, bool onlyErrors=true, bool threadedValidation=true)
        {
            OperationOutcome result = null;
            if (!(resource is Bundle) || !threadedValidation)
            {
                var xmlSerializer = new FhirXmlSerializer();
                //    using (var reader = XDocument.Parse(FhirSerializer.SerializeResourceToXml(resource)).CreateReader())
                using (var reader = XDocument.Parse(xmlSerializer.SerializeToString(resource)).CreateReader())
                {
                    result =  RunValidation(onlyErrors, reader);
                }
            }
            else
            {
                var bundle = (Bundle)resource;
                result =  RunBundleValidation(onlyErrors, bundle);
            }

            if (result.Issue.Count > 0)
            {
                _log.LogWarning("Validation failed");
                _log.LogWarning("Request: " + XDocument.Parse(new FhirXmlSerializer().SerializeToString(resource)));
                _log.LogWarning("Response:" + XDocument.Parse(new FhirXmlSerializer().SerializeToString(result)));                                
            }

            return result;
        }

        private static OperationOutcome RunBundleValidation(bool onlyErrors, Bundle bundle)
        {
            var operationOutcome = new OperationOutcome();

            var itemsRun = new List<string>();
            var serialItems = new List<Resource>();
            var parallellItems = new List<Resource>();
            foreach (var item in bundle.Entry)
            {
                if (itemsRun.Contains(item.Resource.TypeName))
                {
                    parallellItems.Add(item.Resource);
                }
                else
                {
                    serialItems.Add(item.Resource);
                    itemsRun.Add(item.Resource.TypeName);
                }
            }
            RunSerialValidation(onlyErrors, serialItems, operationOutcome);
            RunParallellValidation(onlyErrors, parallellItems, operationOutcome);
            //TODO: Validation of the bundle
            return operationOutcome;
        }

        private static void RunParallellValidation(bool onlyErrors, List<Resource> parallellItems, OperationOutcome operationOutcome)
        {
            var xmlSerializer = new FhirXmlSerializer();
            if (parallellItems.Count > 0)
            {
                Parallel.ForEach(parallellItems, new ParallelOptions {MaxDegreeOfParallelism = parallellItems.Count},
                    loopedResource =>
                    {
                      
                        //using (var reader = XDocument.Parse(FhirSerializer.SerializeResourceToXml(loopedResource))
                        using (var reader = XDocument.Parse(xmlSerializer.SerializeToString(loopedResource))
                   .CreateReader())
                        {
                            var localOperationOutCome = RunValidation(onlyErrors, reader);

                            operationOutcome.Issue.AddRange(localOperationOutCome.Issue);
                        }
                    });
            }
        }

        private static void RunSerialValidation(bool onlyErrors, List<Resource> serialItems, OperationOutcome operationOutcome)
        {
            var xmlSerializer = new FhirXmlSerializer();
            foreach (var item in serialItems)
            {
                var localOperationOutCome = RunValidation(onlyErrors,
                //   XDocument.Parse(FhirSerializer.SerializeResourceToXml(item)).CreateReader());
                    XDocument.Parse(xmlSerializer.SerializeToString(item)).CreateReader());
                operationOutcome.Issue.AddRange(localOperationOutCome.Issue);
            }
        }

        private static OperationOutcome RunValidation(bool onlyErrors, XmlReader reader)
        {
            //var result = _validator.Validate(reader);
            //if (!onlyErrors)
            //{
            //    return result;
            //}
            //var invalidItems = (from item in result.Issue
            //    let error = item.Severity != null && item.Severity.Value == OperationOutcome.IssueSeverity.Error
            //    where error
            //    select item).ToList();

            //result.Issue = invalidItems;
            //return result;
            throw new NotImplementedException();
        }
    }
}
