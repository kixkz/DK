using BookStore.Models.Requests;
using MediatR;

namespace BookStore.Models.MediatR.Commands
{
    public record DublicatedBookCommand(AddBookRequest book) : IRequest<bool>
    {
    }
}
