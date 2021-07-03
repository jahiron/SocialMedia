using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception.GetType() == typeof(BusinessException))
            {
                var exception = new
                {
                    Status = 400,
                    Title = "Bad request",
                    Details = context.Exception.Message
                };

                var json = new
                {
                    errors = new[] { exception }
                };

                context.Result = new BadRequestObjectResult(json);
                context.ExceptionHandled = true;
            }
        }
    }
}
