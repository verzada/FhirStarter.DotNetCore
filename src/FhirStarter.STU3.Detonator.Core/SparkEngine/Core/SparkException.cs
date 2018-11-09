using System;
using System.Net;
using Hl7.Fhir.Model;

namespace FhirStarter.STU3.Detonator.Core.SparkEngine.Core
{
    public class SparkException : Exception
    {
        public HttpStatusCode StatusCode;
        public OperationOutcome Outcome { get; set; }

        public SparkException(HttpStatusCode statuscode, string message = null) : base(message)
        {
            StatusCode = statuscode;
        }
        
        public SparkException(HttpStatusCode statuscode, string message, params object[] values)
            : base(string.Format(message, values))
        {
            StatusCode = statuscode;
        }
        
        public SparkException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public SparkException(HttpStatusCode statuscode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statuscode;
        }

        public SparkException(HttpStatusCode statuscode, OperationOutcome outcome, string message = null)
            : this(statuscode, message)
        {
            Outcome = outcome;
        }
    }
}
