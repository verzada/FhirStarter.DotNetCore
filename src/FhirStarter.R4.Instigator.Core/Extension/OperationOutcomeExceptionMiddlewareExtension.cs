using System.Net;
using System.Web.Http;
using FhirStarter.R4.Detonator.Core.Interface;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FhirStarter.R4.Instigator.Core.Extension
{
    //https://code-maze.com/global-error-handling-aspnetcore/#builtinmiddleware
    //Global Error Handling in ASP.NET Core Web API
    public static class OperationOutcomeExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger<IFhirService> logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"The request {context.Request.Path} failed: {contextFeature.Error}");
                        var issue = new OperationOutcome.IssueComponent
                        {
                            Details = new CodeableConcept(nameof(HttpError), nameof(HttpError),
                                contextFeature.Error.Message)
                        };
                        var outcome = new OperationOutcome();
                        outcome.Issue.Add(issue);
                        
                        var fhirSerializer = new FhirXmlSerializer();
                        var fhirJsonSerializer = new FhirJsonSerializer();
                        var outcomeStr = fhirJsonSerializer.SerializeToString(outcome);

                        await context.Response.WriteAsync(outcomeStr);
                    }
                });
            });
        }
    }
}
