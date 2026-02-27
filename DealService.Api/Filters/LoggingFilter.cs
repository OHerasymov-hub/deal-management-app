using Microsoft.AspNetCore.Mvc.Filters;

namespace DealService.Api.Filters
{
    public class LoggingFilter : Attribute, IActionFilter
    {
        private readonly ILogger<LoggingFilter> _logger;
        private readonly string _title;

        public LoggingFilter(ILogger<LoggingFilter> logger, string title)
        {
            _logger = logger;
            _title = title;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
            _logger.LogInformation("Logging after action execution: {ActionName}, title: {title}", context.ActionDescriptor.DisplayName, _title);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException()
            _logger.LogInformation("Logging before action execution: {ActionName}, title: {title}", context.ActionDescriptor.DisplayName, _title);
        }
    }
}
