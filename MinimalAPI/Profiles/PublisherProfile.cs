using AutoMapper;
using Lib.Entities;
using MinimalAPI.Models;

namespace MinimalAPI.Profiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<Publisher, PublisherDto>();
            CreateMap<PublisherForCreationDto, Publisher>();
            CreateMap<Book, PublisherBooksDto>();
        }
    }
}
