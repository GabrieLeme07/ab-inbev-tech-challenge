namespace Ab.Inbev.Service.Presentation.Application.Extensions.ServiceCollectionExtensions;

public static class AddCorsConfigurationExtension
{
    private const string POLICY_NAME = "_enableCors";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(POLICY_NAME, builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        return services;
    }
}