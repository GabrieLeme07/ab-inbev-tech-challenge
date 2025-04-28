namespace Ab.Inbev.Service.Presentation.Application.Extensions.ServiceCollectionExtensions;

public static class AddApiVersioningConfigurationExtension
{
    private const bool REPORT_API_VERSIONS = true;
    private const string GROUP_NAME_FORMAT = "'v'VVV";
    private const bool SUBSTITUTE_API_VERSION_IN_URL = true;

    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options => options.ReportApiVersions = REPORT_API_VERSIONS);

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = GROUP_NAME_FORMAT;
            options.SubstituteApiVersionInUrl = SUBSTITUTE_API_VERSION_IN_URL;
        });

        return services;
    }
}