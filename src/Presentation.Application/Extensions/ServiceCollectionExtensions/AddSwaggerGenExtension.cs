using Microsoft.OpenApi.Models;

namespace Ab.Inbev.Service.Presentation.Application.Extensions.ServiceCollectionExtensions;

public static class AddSwaggerGenExtension
{
    private const string SWAGGER_DOC_VERSION = "v1";
    private const string SWAGGER_DOC_APLLICATION_TITLE = "Ab.Inbev.Service";

    public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = SWAGGER_DOC_APLLICATION_TITLE,
            Version = SWAGGER_DOC_VERSION
        };

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(SWAGGER_DOC_VERSION, openApiInfo);
        });

        return services;
    }
}