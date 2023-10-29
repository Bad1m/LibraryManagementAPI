using AutoMapper;
using LibraryManagementAPI.Services.Models;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Mappers
{
    public class LibraryMappingProfile : Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<BookViewModel, BookDto>();
            CreateMap<BookDto, BookViewModel>();
            CreateMap<UserViewModel, UserDto>();
            CreateMap<UserDto, UserViewModel>();
            CreateMap<AuthenticateViewModel, AuthenticateRequest>();
        }
    }
}
