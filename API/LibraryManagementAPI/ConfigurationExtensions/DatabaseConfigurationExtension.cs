using LibraryManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.ConfigurationExtensions
{
    public static class DatabaseConfigurationExtension
    {
        public static IServiceCollection ConfigureLibraryDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LibraryDatabase")));

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibraryContext>();
                dbContext.Database.Migrate();
            }

            return services;
        }
    }
}