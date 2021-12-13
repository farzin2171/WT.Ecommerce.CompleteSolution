using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Morcatko.AspNetCore.JsonMergePatch;
using System;
using System.Collections.Generic;
using System.Linq;


namespace WT.Ecommerce.Infrastructure.ApiVersion
{
	/// <summary>
	/// This filter wth replace the JsonMergePatchDocument type in the OpenAPI specification with the real input type intended for the endpoint.
	/// 
	/// Code taken from https://github.com/Morcatko/Morcatko.AspNetCore.JsonMergePatch/blob/master/src/3.0-testApp/JsonMergePatchDocumentOperationFilter.cs
	/// but was slightly modified.
	/// </summary>
	public class JsonMergePatchDocumentOperationFilter : IOperationFilter
	{
		private static bool IsJsonMergePatchDocumentType(Type t) => t is { IsGenericType: true } && t.GetGenericTypeDefinition() == typeof(JsonMergePatchDocument<>);
		private static bool IsIEnumerableType(Type t) => t is { IsGenericType: true } && t.GetGenericTypeDefinition() == typeof(IEnumerable<>);

		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			OpenApiSchema GenerateSchema(Type type)
				=> context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

			void CleanUpSchemas(string jsonMergePatchSchemaId)
			{
				var schemas = context.SchemaRepository.Schemas;
				if (schemas.TryGetValue(jsonMergePatchSchemaId, out var jsonMergePatchSchema))
				{
					var operationsSchema = jsonMergePatchSchema.Properties["operations"];
					schemas.Remove(jsonMergePatchSchemaId);
					schemas.Remove(operationsSchema.Items.Reference.Id);
				}
			}

			var bodyParameters = context.ApiDescription.ParameterDescriptions.Where(p => p.Source == BindingSource.Body).ToList();

			foreach (var parameter in bodyParameters)
			{
				if (IsJsonMergePatchDocumentType(parameter.Type))
				{
					CleanUpSchemas(operation.RequestBody.Content[JsonMergePatchDocument.ContentType].Schema.Reference.Id);
					operation.RequestBody.Content[JsonMergePatchDocument.ContentType].Schema = GenerateSchema(parameter.Type.GenericTypeArguments[0]);
				}
				else if (IsIEnumerableType(parameter.Type) && IsJsonMergePatchDocumentType(parameter.Type.GenericTypeArguments[0]))
				{
					var enumerableType = typeof(IEnumerable<>);
					var genericEnumerableType = enumerableType.MakeGenericType(parameter.Type.GenericTypeArguments[0]);

					CleanUpSchemas(operation.RequestBody.Content[JsonMergePatchDocument.ContentType].Schema.Items.Reference.Id);
					operation.RequestBody.Content[JsonMergePatchDocument.ContentType].Schema = GenerateSchema(genericEnumerableType);
				}
			}
		}
	}
}
