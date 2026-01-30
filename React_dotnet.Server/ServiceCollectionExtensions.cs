using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using React_dotnet.Server.Dtos.Options;
using System.Text;

namespace React_dotnet.Server
{
    public static class ServiceCollectionExtensions
    {
        //Extension method
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfigSection = configuration.GetSection(nameof(JwtOptions));
            // Options pattern, inject
            services.Configure<JwtOptions>(jwtConfigSection);

            var jwtConfig = jwtConfigSection.Get<JwtOptions>() ?? new JwtOptions();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero, // Alapból a lejárat után 5 percig, így meg nem
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
