using System;

namespace VoteApp.Application.Commons.Exceptions
{
    public class BadRequestError
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }
    }
    public class BadRequestException : Exception
    {
        public BadRequestError BadRequestError { get; set; }
        public BadRequestException(string message) : base(message)
        {
            BadRequestError = new BadRequestError()
            {
                Title = "An error occurred while processing your request.",
                Status = 400,
                Error = message
            };
        }
    }

}
