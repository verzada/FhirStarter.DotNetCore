using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;

namespace FhirStarter.R4.Detonator.Core.Filter
{
    public static class ErrorHandlingMiddleware
    {
        public static Resource GetOperationOutCome(Exception exception, bool ShowStackTrace)
        {
            var operationOutCome = new OperationOutcome {Issue = new List<OperationOutcome.IssueComponent>()};
            var issue = new OperationOutcome.IssueComponent
            {
                Severity = OperationOutcome.IssueSeverity.Fatal,
                Code = OperationOutcome.IssueType.NotFound,
                Details = new CodeableConcept("StandardException", exception.GetType().ToString(), exception.Message),
            };

            if (ShowStackTrace)
            {
                issue.Diagnostics = exception.StackTrace;
            }

            var responseIssue = CheckForHttpResponseException(exception, ShowStackTrace);
            if (responseIssue != null)
                operationOutCome.Issue.Add(responseIssue);

            operationOutCome.Issue.Add(issue);
            return operationOutCome;
        }

        private static OperationOutcome.IssueComponent CheckForHttpResponseException(Exception exception, bool ShowStackTrace)
        {
            OperationOutcome.IssueComponent responseIssue = null;
            //todo
            //if (exception.GetType().ToString().Contains(nameof(iactionre)))
            //{
            //    var responseException = (HttpResponseException)exception;

            //    if (responseException.Response != null)
            //    {
            //        responseIssue = new OperationOutcome.IssueComponent
            //        {
            //            Severity = OperationOutcome.IssueSeverity.Fatal,
            //            Code = OperationOutcome.IssueType.Exception,
            //            Details =
            //                new CodeableConcept("Response", exception.GetType().ToString(),
            //                    responseException.Response.ReasonPhrase)
            //        };
            //        if (ShowStackTrace)
            //        {
            //            responseIssue.Diagnostics = exception.StackTrace;
            //        }
            //    }
            //}
            return responseIssue;
        }
    }
}
