using BookStore.DL.Interfaces;
using BookStore.Models.MediatR.Commands;
using MediatR;

namespace BookStore.BL.CommandHandlers
{
    public class DublicatedBookCommandHandler : IRequestHandler<DublicatedBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;

        public DublicatedBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<bool> Handle(DublicatedBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookRepository.IsBookDuplicated(request.book);
        }
    }
}
