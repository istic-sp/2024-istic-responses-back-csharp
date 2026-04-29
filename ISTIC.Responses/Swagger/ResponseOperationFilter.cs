using ISTIC.Responses.Core;
using ISTIC.Responses.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ISTIC.Responses.Swagger;

public class ResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            returnType = returnType.GenericTypeArguments[0];
        }

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseOf<>))
        {
            var successReturnType = returnType.GenericTypeArguments[0];

            if (successReturnType.IsGenericType && successReturnType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                return;

            ConfigureResponses(operation, successReturnType, typeof(Error));
        }
        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(CustomResponseOf<,>))
        {
            var successReturnType = returnType.GenericTypeArguments[0];
            var errorDataType = returnType.GenericTypeArguments[1];
            var customErrorType = typeof(CustomError<>).MakeGenericType(errorDataType);

            if (successReturnType.IsGenericType && successReturnType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                return;

            ConfigureResponses(operation, successReturnType, customErrorType);
        }
        else if (returnType == typeof(Response))
            ConfigureResponses(operation, null, typeof(Error));
    }

    private void ConfigureResponses(OpenApiOperation operation, Type successReturnType, Type errorType)
    {
        var errorMediaType = CreateErrorMediaType(errorType);

        if (successReturnType != null)
        {
            var successMediaType = CreateSuccessMediaType(successReturnType);
            operation.Responses["200"] = new OpenApiResponse
            {
                Content =
                {
                    { "application/json", successMediaType }
                }
            };
        }
        else
            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "No content",
                Content = new Dictionary<string, OpenApiMediaType>()
            };

        operation.Responses["400"] = new OpenApiResponse
        {
            Content =
            {
                { "application/json", errorMediaType }
            }
        };
    }

    private OpenApiMediaType CreateSuccessMediaType(Type successReturnType)
    {
        return new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = successReturnType.GetSchemaId(),
                    Type = ReferenceType.Schema
                }
            }
        };
    }

    private OpenApiMediaType CreateErrorMediaType(Type errorType)
    {
        return new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = errorType.GetSchemaId(),
                    Type = ReferenceType.Schema
                }
            }
        };
    }
}