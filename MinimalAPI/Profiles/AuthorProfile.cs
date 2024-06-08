using AutoMapper;
using Lib.Entities;
using MinimalAPI.Models;

namespace MinimalAPI.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorForCreationDto, Author>();
        }

    }
}
