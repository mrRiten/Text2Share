using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using TextMicroService.Core.Models;

namespace TextMicroService.Core.Attributes
{
    public class ValidateSourceFilter(IOptions<XSource> options) : IAsyncActionFilter
    {
        private readonly XSource _xSource = options.Value;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var expectedToken = _xSource.Token;

            if (!request.Headers.TryGetValue("X-Source", out var providedToken) || providedToken != expectedToken)
            {
                context.Result = new BadRequestResult();
                return;
            }

            await next();
        }
    }
}
