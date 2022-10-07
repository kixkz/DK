using BookStore.Models.Requests;
using BookStore.Models.Responses;
using MediatR;

namespace BookStore.Models.MediatR.Commands
{
    public record UpdateBookCommand(UpdateBookRequest book) : IRequest<AddBookResponse>
    {
    }
}
