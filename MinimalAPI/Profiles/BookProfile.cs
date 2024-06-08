using AutoMapper;
using Lib.Entities;
using MinimalAPI.Models;

namespace MinimalAPI.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookForCreationDto, Book>();
        }
    }
}
