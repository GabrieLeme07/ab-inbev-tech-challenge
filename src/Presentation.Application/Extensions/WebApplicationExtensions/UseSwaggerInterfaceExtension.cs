namespace Ab.Inbev.Service.Presentation.Application.Extensions.WebApplicationExtensions;

public static class UseSwaggerInterfaceExtension
{
    private const string SWAGGER_ENDPOINT_URL = "/swagger/v1/swagger.json";
    private const string SWAGGER_ENDPOINT_NAME = "Ab.Inbev.Service";
    private const string SWAGGER_ROUTE_PREFIX = "";

    public static WebApplication UseSwaggerInterface(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(SWAGGER_ENDPOINT_URL, SWAGGER_ENDPOINT_NAME);
            options.RoutePrefix = SWAGGER_ROUTE_PREFIX;
        });

        return app;
    }
}