using AutoMapper;
using BookStore.Models.Models;
using BookStore.Models.Requests;

namespace BookStore.AutoMapper
{
    internal class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AddAuthorRequest, Author>();
            CreateMap<AddBookRequest, Book>();
            CreateMap<AddPersonRequest, Person>();
            CreateMap<UpdateAuthorRequest, Author>();
        }
    }
}
