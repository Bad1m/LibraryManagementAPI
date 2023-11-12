using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Filters
{
    public class CancellationTokenFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            try
            {
                await next();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw new ApplicationException("The request was canceled by the user.");
            }
        }
    }
}