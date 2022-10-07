using BookStore.DL.Interfaces;
using BookStore.Models.MediatR.Commands;
using MediatR;

namespace BookStore.BL.CommandHandlers
{
    public class AddMultipleAuthorsCommandHandler : IRequestHandler<AddMultipleAuthorsCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddMultipleAuthorsCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<bool> Handle(AddMultipleAuthorsCommand request, CancellationToken cancellationToken)
        {
            return await _authorRepository.AddMultipleAuthors(request.authosCollection);
        }
    }
}
