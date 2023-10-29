using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Services;
using LibraryManagementAPI.Services.Mappers;
using LibraryManagementAPI.Mappers;
using LibraryManagementAPI.Data.Interfaces;
using LibraryManagementAPI.Data.Repositories;
using LibraryManagementAPI.Services.Validators;
using LibraryManagementAPI.Data.Validators;

namespace LibraryManagementAPI.ConfigurationExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLibraryServices(this IServiceCollection services)
        {
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddTransient<IBookValidator, BookValidator>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();

            services.AddAutoMapper(typeof(LibraryMappingProfile));
            services.AddAutoMapper(typeof(ServicesMappingProfile));

            return services;
        }
    }
}