﻿using System.Net.Http;

namespace FhirStarter.STU3.Detonator.DotNetCore.SparkEngine.Filters
{
    public class FhirResponseHandler : DelegatingHandler
    {
        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    return base.SendAsync(request, cancellationToken).ContinueWith(
        //        task =>
        //        {
        //            if (task.IsCompleted)
        //            {
        //                FhirResponse fhirResponse;
        //                if (task.Result.TryGetContentValue(out fhirResponse))
        //                {
        //                    return request.CreateResponse(fhirResponse);
        //                }
        //                return task.Result;
        //            }
        //            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //            //return task.Result;
        //        }, 
        //        cancellationToken
        //    );
        //}
    }
}
