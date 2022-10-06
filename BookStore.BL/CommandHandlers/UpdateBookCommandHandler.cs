using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.MediatR.Commands;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;
using MediatR;

namespace BookStore.BL.CommandHandlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, AddBookResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<AddBookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var auth = await _bookRepository.GetByID(request.book.Id);
            if (auth == null)
                return new AddBookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Book not exist"
                };
            var book = _mapper.Map<Book>(request.book);

            var result = _bookRepository.UpdateBook(book);
            return new AddBookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = await result
            };
        }
    }
}
