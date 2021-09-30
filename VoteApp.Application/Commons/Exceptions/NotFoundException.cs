using System;

namespace VoteApp.Application.Commons.Exceptions
{
    public class NotFoundError
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }
    }
    public class NotFoundException : Exception
    {
        public NotFoundError NotFoundError { get; set; }
        public NotFoundException(string message) : base(message)
        {
            NotFoundError = new NotFoundError()
            {
                Title = "The specify resource was not found",
                Status = 404,
                Error = message
            };
        }
        public NotFoundException(string name, object key) : this($"{name} was not found with key {key}")
        {
            NotFoundError.Error = $"{name} was not found with key {key}";
        }
    }

}
