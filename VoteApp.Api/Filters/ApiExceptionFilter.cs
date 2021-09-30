using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using VoteApp.Application.Commons.Exceptions;

namespace VoteApp.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandler;
        public ApiExceptionFilter()
        {
            _exceptionHandler = new Dictionary<Type, Action<ExceptionContext>>
            {
                {typeof(ValidateException), HandleValidateException },
                {typeof(NotFoundException), HandleNotFoundException },
                {typeof(BadRequestException), HandeBadRequestException },
            };
        }

        public void OnException(ExceptionContext context)
        {
            HandleException(context);
            //logging here
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandler.ContainsKey(type))
            {
                _exceptionHandler[type].Invoke(context);
                return;
            }
            HandlerInternalServerException(context);
        }

        private void HandeBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;
            context.Result = new BadRequestObjectResult(exception.BadRequestError);
            context.ExceptionHandled = true;
        }

        private void HandleValidateException(ExceptionContext context)
        {
            var exception = context.Exception as ValidateException;
            context.Result = new BadRequestObjectResult(exception.ValidateError);
            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;
            context.Result = new NotFoundObjectResult(exception.NotFoundError);
            context.ExceptionHandled = true;
        }

        private void HandlerInternalServerException(ExceptionContext context)
        {
            var exception = context.Exception;
            context.Result = new ObjectResult(new { title = "Internal server error", status = 500, error = exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
