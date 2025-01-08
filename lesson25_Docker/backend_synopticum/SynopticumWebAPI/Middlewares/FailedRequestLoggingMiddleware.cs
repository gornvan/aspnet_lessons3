using System.Text;

namespace SynopticumWebAPI.Middlewares;

public class BadRequestLoggingMiddleware(
    RequestDelegate _next,
    ILogger<BadRequestLoggingMiddleware> _logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        // Store the original response body stream
        var originalBodyStream = context.Response.Body;

        // Create a new memory stream to capture the response body
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        // Call the next middleware in the pipeline
        await _next(context);

        // Check if the response status code is BadRequest
        if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
            // Read the response body from the memory stream

            context.Response.Body.Position = 0; // Ensure we start reading from the beginning
            var responseText = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();
            context.Response.Body.Position = 0; // Reset the stream position again

            var sourceIp = context.Connection.RemoteIpAddress;
            _logger.LogWarning($"A badly formed request has been submitted by {sourceIp}; errors: {responseText}");

            // Create a new stream from the errorsText
            var responseBytes = Encoding.UTF8.GetBytes(responseText);
            context.Response.Body = originalBodyStream; // Restore the original stream

            // Write the response back to the original stream
            await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
            return; // Exit to prevent further processing
        }

        // Copy the contents of the memory stream (responseBody) to the original stream
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }
}
