using System;
using System.Collections.Generic;
using System.Text;

namespace FhirStarter.R4.Instigator.Core.Validation.Exceptions
{
    public class ValidateOutputException : ArgumentException
    {

        public ValidateOutputException(string message) : base(message)
        {
        }

        public ValidateOutputException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
