using BookStore.Models.Models;
using MediatR;

namespace BookStore.Models.MediatR.Commands
{
    public record AddMultipleAuthorsCommand(IEnumerable<Author> authosCollection) : IRequest<bool>
    {
    }
}
