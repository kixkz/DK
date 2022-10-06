using BookStore.Models.Models;
using MediatR;

namespace BookStore.Models.MediatR.Commands
{
    public record GetBookByIdCommand(int bookId) : IRequest<Book>
    {

    }
}
