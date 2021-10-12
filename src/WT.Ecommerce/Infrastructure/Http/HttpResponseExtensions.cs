using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace WT.Ecommerce.Infrastructure.Http
{
	public static class HttpResponseExtensions
	{
		public static async Task WriteBadRequestResponseAsync(this HttpResponse response, string title, string detail = null)
		{
			var problemDetails = new ApiProblemDetails(HttpStatusCode.BadRequest, title, detail);
			await WriteResponseAsync(response, problemDetails);
		}

		public static async Task WriteBadRequestResponseAsync(this HttpResponse response, ModelStateDictionary modelState)
		{
			var problemDetails = new ApiProblemDetails(HttpStatusCode.BadRequest, modelState);
			await WriteResponseAsync(response, problemDetails);
		}

		public static async Task WriteNotFoundResponseAsync(this HttpResponse response, string title, string detail = null)
		{
			var problemDetails = new ApiProblemDetails(HttpStatusCode.NotFound, title, detail);
			await WriteResponseAsync(response, problemDetails);
		}

		public static async Task WriteUnauthorizedResponseAsync(this HttpResponse response, string title, string detail = null)
		{
			var problemDetails = new ApiProblemDetails(HttpStatusCode.Unauthorized, title, detail);
			await WriteResponseAsync(response, problemDetails);
		}

		public static async Task WriteForbiddenResponseAsync(this HttpResponse response, string title, string detail = null)
		{
			var problemDetails = new ApiProblemDetails(HttpStatusCode.Forbidden, title, detail);
			await WriteResponseAsync(response, problemDetails);
		}

		public static async Task WriteInternalServerErrorResponseAsync(this HttpResponse response, string title, string detail = null)
		{
			var problemDetails = new ApiProblemDetails(HttpStatusCode.InternalServerError, title, detail);
			await WriteResponseAsync(response, problemDetails);
		}

		public static async Task WriteResponseAsync(this HttpResponse response, ApiProblemDetails problemDetails)
		{
			response.StatusCode = problemDetails.Status.GetValueOrDefault();

			// Make sure problem responses are never cached.
			if (!response.Headers.ContainsKey(HeaderNames.CacheControl))
			{
				response.Headers.Add(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
			}
			if (!response.Headers.ContainsKey(HeaderNames.Pragma))
			{
				response.Headers.Add(HeaderNames.Pragma, "no-cache");
			}
			if (!response.Headers.ContainsKey(HeaderNames.Expires))
			{
				response.Headers.Add(HeaderNames.Expires, "0");
			}

			await response.WriteJsonAsync(problemDetails, "application/problem+json");
		}

		public static async Task WriteJsonAsync<T>(this HttpResponse response, T obj, string contentType = null)
		{
			response.ContentType = contentType ?? "application/json";

			var options = new JsonSerializerOptions
			{
				IgnoreNullValues = true
			};
			var value = JsonSerializer.Serialize(obj, options);

			await using var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8);
			await writer.WriteAsync(value);
		}
	}
}
