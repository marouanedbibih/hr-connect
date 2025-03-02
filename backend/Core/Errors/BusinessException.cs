using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace backend.Core.Errors
{
    public class BusinessException : Exception
    {
        public List<OwnFieldError> FieldErrors { get; set; }
        public Dictionary<string, string> Error { get; set; }
        public int Status { get; set; }

        // Constructor for message
        public BusinessException(string message, int status)
            : base(message)
        {
            Status = status;
            Error = new Dictionary<string, string> { { "message", message } };
        }

        // Constructor for one fieldError
        public BusinessException(string field, string message, int status)
            : base(message)
        {
            FieldErrors = new List<OwnFieldError> { new OwnFieldError(field, message) };
            Status = status;
        }

        // Constructor with fieldErrors
        public BusinessException(List<OwnFieldError> fieldErrors, int status)
        {
            FieldErrors = fieldErrors;
            Status = status;
        }

        // Constructor with error dictionary
        public BusinessException(Dictionary<string, string> error, int status)
        {
            Error = error;
            Status = status;
        }
    }
}