using LibraryManagementAPI.Data.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibraryManagementAPI.ConfigurationExtensions
{
    public static class JWTConfiguration
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfigSection = configuration.GetSection("Auth");

            var authSettings = new AuthSettings();
            configuration.Bind(nameof(authSettings), authSettings);
            services.AddSingleton(authSettings);

            services.Configure<AuthSettings>(authConfigSection);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });
            services.AddAuthorization();

            return services;
        }
    }
}