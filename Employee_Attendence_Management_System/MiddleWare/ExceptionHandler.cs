using System.Net;
using System.Text.Json;

namespace Employee_Attendence_Management_System.MiddleWare;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        HttpStatusCode code = HttpStatusCode.InternalServerError;
        string message = "Something went wrong. Please try again.";

        _logger.LogError(exception, "An unhandled exception occurred.");

        bool isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        if (isAjax)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.Headers.Add("X-Error", "true");

            var jsonResponse = new
            {
                success = false,
                statusCode = (int)code,
                error = message
            };

            string jsonMessage = JsonSerializer.Serialize(jsonResponse);
            await context.Response.WriteAsync(jsonMessage);
        }
        else
        {
            var redirectUrl = $"/Home/HandleErrorWithToaster?message={Uri.EscapeDataString(message)}";
            _logger.LogInformation("Redirecting to: {RedirectUrl}", redirectUrl);
            context.Response.StatusCode = (int)HttpStatusCode.Redirect; // 302 To Temporary redirect
            context.Response.Headers["Location"] = redirectUrl;
            await context.Response.CompleteAsync();
        }
    }
}
