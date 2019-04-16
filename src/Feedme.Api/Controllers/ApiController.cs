using System;
using Microsoft.AspNetCore.Mvc;
using Feedme.Api.Utils;
using Feedme.Application.RequestHandling;
using Feedme.Domain.Functional;

namespace Feedme.Api.Controllers
{
    public abstract class ApiController : Controller
    {
        protected readonly RequestHandler _handler;

        public ApiController(RequestHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected IActionResult FromResult(Result result)
        {
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        protected IActionResult FromResult<T>(Result<T> result)
        {
            return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
        }
    }
}