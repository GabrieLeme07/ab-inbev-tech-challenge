using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ab.Inbev.Service.Presentation.Application.Extensions.ServiceCollectionExtensions;

public static class AddSecurityConfigurationExtension
{
    public static IServiceCollection AddJWTConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
