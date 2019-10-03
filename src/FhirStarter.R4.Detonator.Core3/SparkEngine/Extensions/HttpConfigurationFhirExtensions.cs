﻿using FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.ExceptionHandling;
using FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Filters;
using FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Formatters;
using FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Handlers;

namespace FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Extensions
{
    public static class HttpConfigurationFhirExtensions
    {
        private static void AddFhirFormatters(this HttpConfiguration config, bool clean = true)
        {
            // remove existing formatters
            if (clean) config.Formatters.Clear();

            // Hook custom formatters            
            config.Formatters.Add(new XmlFhirFormatter());
            config.Formatters.Add(new JsonFhirFormatter());
            config.Formatters.Add(new BinaryFhirFormatter());
            config.Formatters.Add(new HtmlFhirFormatter());

            // Add these formatters in case our own throw exceptions, at least you
            // get a decent error message from the default formatters then.
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.Add(new XmlMediaTypeFormatter());
        }

        private static void AddFhirMessageHandlers(this HttpConfiguration config)
        {
            // TODO: Should compression handler be before InterceptBodyHandler.  Have not checked.
            config.MessageHandlers.Add((new CompressionHandler()));
            config.MessageHandlers.Add(new FhirMediaTypeHandler());
          //  config.MessageHandlers.Add(new FhirResponseHandler());
            config.MessageHandlers.Add(new FhirErrorMessageHandler());

        }

        public static void AddFhir(this HttpConfiguration config)
        {

            config.AddFhirMessageHandlers();

            // Hook custom formatters            
            config.AddFhirFormatters();

            // EK: Remove the default BodyModel validator. We don't need it,
            // and it makes the Validation framework throw a null reference exception
            // when the body empty. This only surfaces when calling a DELETE with no body,
            // while the action method has a parameter for the body.
            config.Services.Replace(typeof(IBodyModelValidator), null);
        }
    }
}
