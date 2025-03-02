// using Microsoft.AspNetCore.Http;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;
// using System.Text.Json;
// using System.ComponentModel.DataAnnotations;

// public class GlobalExceptionMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly ILogger<GlobalExceptionMiddleware> _logger;

//     public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
//     {
//         _next = next;
//         _logger = logger;
//     }

//     public async Task InvokeAsync(HttpContext httpContext)
//     {
//         try
//         {
//             await _next(httpContext);
//         }
//         catch (ValidationException validationException)
//         {
//             // Handle validation exception
//             await HandleValidationExceptionAsync(httpContext, validationException);
//         }
//         catch (Exception ex)
//         {
//             // Handle generic exception
//             await HandleExceptionAsync(httpContext, ex);
//         }
//     }

//     private Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
//     {
//         var errors = new Dictionary<string, string[]>();

//         // Assuming ValidationException contains a dictionary of errors
//         foreach (var error in exception.Errors)
//         {
//             errors[error.Key] = error.Value;
//         }

//         context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//         context.Response.ContentType = "application/json";

//         var response = new
//         {
//             Status = "Error",
//             Message = "Validation failed.",
//             Errors = errors
//         };

//         var jsonResponse = JsonSerializer.Serialize(response);
//         return context.Response.WriteAsync(jsonResponse);
//     }

//     private Task HandleExceptionAsync(HttpContext context, Exception exception)
//     {
//         _logger.LogError(exception, exception.Message);

//         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//         context.Response.ContentType = "application/json";

//         var response = new
//         {
//             Status = "Error",
//             Message = "An unexpected error occurred.",
//             Details = exception.Message
//         };

//         var jsonResponse = JsonSerializer.Serialize(response);
//         return context.Response.WriteAsync(jsonResponse);
//     }
// }
