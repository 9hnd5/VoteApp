using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VoteApp.Application.Commons.Exceptions
{
    public class ValidateExceptionError
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public IDictionary<string, string[]> Error { get; set; }
    }
    public class ValidateException : Exception
    {
        public ValidateExceptionError ValidateError { get; set; }
        public ValidateException(IEnumerable<ValidationFailure> failures) : base()
        {
            var error = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
            ValidateError = new ValidateExceptionError() { Title = "Validation Error", Status = 400, Error = error };
        }
    }

}
