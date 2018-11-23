/* 
 * Copyright (c) 2014, Furore (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.Net.Http;

namespace FhirStarter.Bonfire.DotNetCore.SparkEngine.ExceptionHandling
{
    public interface IExceptionResponseMessageFactory
    {
        HttpResponseMessage GetResponseMessage(Exception exception, HttpRequestMessage reques);
    }
}
