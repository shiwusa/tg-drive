using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TgDrive.Web.Auth;

namespace TgDrive.Web.HttpApi;

public class AuthHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = AuthorizationConsts.HashHeaderName,
            In = ParameterLocation.Header,
            Description = "Telegram authorization hash signature",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
            }
        });

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = AuthorizationConsts.DataHeaderName,
            In = ParameterLocation.Header,
            Description = "Telegram user credentials",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
            }
        });
    }
}
