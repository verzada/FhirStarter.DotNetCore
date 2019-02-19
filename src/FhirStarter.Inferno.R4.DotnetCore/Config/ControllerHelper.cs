using System;
using System.Collections.Generic;
using System.Linq;
using FhirStarter.Bonfire.DotNetCore.Interface;
using Hl7.Fhir.Model;
using Microsoft.Extensions.DependencyInjection;

namespace FhirStarter.Inferno.STU3.DotnetCore.Config
{
    public static class ControllerHelper
    {
        public static IFhirService GetFhirService(string type, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(
                    $"The service provider cannot be null when looking for a {nameof(IFhirService)}");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(
                    $"The parameter {type} cannot be null, it must contain a valid {nameof(Hl7)} reference to a valid {nameof(Resource)} such as {nameof(Patient)}");
            }

            var request = serviceProvider.GetService<IEnumerable<IFhirService>>()
                .FirstOrDefault(p => p.GetServiceResourceReference().Equals(type));
            return request;
        }
    }
}