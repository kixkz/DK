using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.MediatR.Commands;
using BookStore.Models.Models;
using BookStore.Models.Responses;
using MediatR;

namespace BookStore.BL.CommandHandlers
{
    public class GetBookByIdCommandHandler : IRequestHandler<GetBookByIdCommand, Book?>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book?> Handle(GetBookByIdCommand request, CancellationToken cancellationToken)
        {
            return await _bookRepository.GetByID(request.bookId);
        }
    }
}
