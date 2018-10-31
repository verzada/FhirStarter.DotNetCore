using System;
using System.Net.Http;

namespace FhirStarter.Bonfire.DotNetCore.SparkEngine.ExceptionHandling
{
    public interface IExceptionResponseMessageFactory
    {
        HttpResponseMessage GetResponseMessage(Exception exception, HttpRequestMessage reques);
    }
}
