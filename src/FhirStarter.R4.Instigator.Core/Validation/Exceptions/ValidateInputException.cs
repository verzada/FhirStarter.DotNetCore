using System;
using System.Collections.Generic;
using System.Text;

namespace FhirStarter.R4.Instigator.Core.Validation.Exceptions
{
    public class ValidateInputException : ArgumentException
    {
        public ValidateInputException(string message) : base(message)
        {
        }

        public ValidateInputException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
