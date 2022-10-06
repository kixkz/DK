using BookStore.Models.Models;
using MediatR;

namespace BookStore.Models.MediatR.Commands
{
    public record GetAuthorByIdCommand(int authorId) : IRequest<Author?>
    {
    }
}
