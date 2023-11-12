using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Services;
using LibraryManagementAPI.Services.Mappers;
using LibraryManagementAPI.Data.Interfaces;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Services.Validators;
using LibraryManagementAPI.Data.Validators;
using LibraryManagementAPI.Filters;

namespace LibraryManagementAPI.ConfigurationExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLibraryServices(this IServiceCollection services)
        {
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddTransient<IBookValidator, BookValidator>();
            services.AddScoped<CancellationTokenFilter>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IJWTService, JWTService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();

            services.AddAutoMapper(typeof(ServicesMappingProfile));
            return services;
        }
    }
}