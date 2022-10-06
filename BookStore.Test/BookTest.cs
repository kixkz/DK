using AutoMapper;
using BookStore.AutoMapper;
using BookStore.BL.Services;
using BookStore.Controllers;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookStore.Test
{
    public class BookTest
    {
        private IList<Book> _books = new List<Book>()
        {
            new Book()
            {
                Id = 1,
                Title = "New book",
                Quantity = 1,
                LastUpdated = DateTime.Now,
                Price = 100
            },
            new Book()
            {
                Id = 2,
                AuthorId = 2,
                Title = "Another book",
                Quantity = 2,
                LastUpdated = DateTime.Now,
                Price = 50
            }
        };

        private readonly IMapper _mapper;
        private Mock<ILogger<BookService>> _loggerMock;
        private Mock<ILogger<BookController>> _loggerBookControllerMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;

        public BookTest()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });

            _mapper = mockMapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<BookService>>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerBookControllerMock = new Mock<ILogger<BookController>>();
        }

        [Fact]
        public async Task Book_GetAll_Count_Check()
        {
            //setup
            var expectedCount = 2;

            _bookRepositoryMock.Setup(x => x.GetAllBooks()).ReturnsAsync(_books);

            //inject
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object, _loggerMock.Object);

            var controller = new BookController(_loggerBookControllerMock.Object, service);

            //act
            var result = await controller.Get();

            var okObjectResult = result as OkObjectResult;

            var books = okObjectResult.Value as IEnumerable<Book>;

            //Assert
            Assert.NotNull(books);
            Assert.NotEmpty(books);
            Assert.Equal(expectedCount, books.Count());
            Assert.Equal(books, _books);
        }

        [Fact]
        public async Task Book_GetBookById_NotFound()
        {
            //setup
            var bookId = 3;
            var expectedBook = _books.FirstOrDefault(x => x.Id == bookId);

            _bookRepositoryMock.Setup(x => x.GetByID(bookId)).ReturnsAsync(expectedBook);

            //inject
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object, _loggerMock.Object);

            var controller = new BookController(_loggerBookControllerMock.Object, service);

            //act
            var result = await controller.GetById(bookId);

            //Assert
            var notObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notObjectResult);

            var returnedBookId = (int)notObjectResult.Value;
            Assert.NotNull(returnedBookId);
            Assert.Equal(bookId, returnedBookId);
        }

        [Fact]
        public async Task AddBookOk()
        {
            //setup
            var bookRequest = new AddBookRequest()
            {
                Title = "Goshko",
                AuthorId = 3,
                Quantity = 20,
                LastUpdated = DateTime.Now,
                Price = 60
            };

            var exxpectedBookId = 3;

            _bookRepositoryMock.Setup(x => x.AddBook(It.IsAny<Book>())).Callback(() =>
            {
                _books.Add(new Book()
                {
                    Id = exxpectedBookId,
                    Title = bookRequest.Title,
                    AuthorId = bookRequest.AuthorId,
                    Quantity = bookRequest.Quantity,
                    LastUpdated= DateTime.Now,
                    Price = bookRequest.Price,
                });
            }).ReturnsAsync(() => _books.FirstOrDefault(x => x.Id == exxpectedBookId));

            //inject
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object, _loggerMock.Object);

            var controller = new BookController(_loggerBookControllerMock.Object, service);

            //act
            var result = await controller.AddBook(bookRequest);

            //asset
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as AddBookResponse;
            Assert.NotNull(resultValue);
            Assert.Equal(exxpectedBookId, resultValue.Book.Id);
        }

        [Fact]
        public async Task Author_DeleteOk()
        {
            //setup
            var bookId = 1;

            var bookToDelete = _books.FirstOrDefault(x => x.Id == bookId);

            _bookRepositoryMock.Setup(x => x.DeleteBook(bookId)).Callback(() =>
            {
                _books.Remove(_books.FirstOrDefault(x => x.Id == bookId));
            }).ReturnsAsync(() => bookToDelete);

            //inject
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object, _loggerMock.Object);

            var controller = new BookController(_loggerBookControllerMock.Object, service);

            //act
            var result = await controller.Delete(bookId);

            var okObjectResult = result as OkObjectResult;

            //asset
            Assert.NotNull(okObjectResult);
            Assert.Equal(1, _books.Count);

            var resultValue = okObjectResult.Value as Book;

            Assert.NotNull(resultValue);
            Assert.Equal(bookToDelete, resultValue);
        }

        [Fact]
        public async Task Author_DeleteBadRequest()
        {
            //setup
            var bookId = 3;

            var bookToDelete = _books.FirstOrDefault(x => x.Id == bookId);

            _bookRepositoryMock.Setup(x => x.DeleteBook(bookId)).ReturnsAsync(() => bookToDelete);

            //inject
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object, _loggerMock.Object);

            var controller = new BookController(_loggerBookControllerMock.Object, service);

            //act
            var result = await controller.Delete(bookId);

            var badRequestObjectResult = result as BadRequestObjectResult;

            //asset
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(2, _books.Count);

            var resultValue = badRequestObjectResult.Value as Book;

            Assert.Null(resultValue);
            Assert.Equal(bookToDelete, resultValue);
        }

        [Fact]
        public async Task UpdateBookOk()
        {
            //setup
            var bookRequest = new UpdateBookRequest()
            {
                Id = 1,
                Title = "Toshko",
                AuthorId = 3,
                Quantity = 20,
                LastUpdated = DateTime.Now,
                Price = 60
            };


            _bookRepositoryMock.Setup(x => x.GetByID(bookRequest.Id)).ReturnsAsync(new Book());
            _bookRepositoryMock.Setup(x => x.UpdateBook(It.IsAny<Book>())).Callback(() =>
            {
                var book = _books.FirstOrDefault(x => x.Id == bookRequest.Id);
                book.Title = bookRequest.Title;
                book.AuthorId = bookRequest.AuthorId;
                book.Quantity = bookRequest.Quantity;
                book.LastUpdated = bookRequest.LastUpdated;
                book.Price = bookRequest.Price;
            }).ReturnsAsync(() => _books.FirstOrDefault(x => x.Id == bookRequest.Id));

            //inject
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object, _loggerMock.Object);

            var controller = new BookController(_loggerBookControllerMock.Object, service);

            //act
            var result = await controller.Update(bookRequest);

            //asset
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as AddBookResponse;

            Assert.NotNull(resultValue);
            Assert.Equal(bookRequest.Title, resultValue.Book.Title);
            Assert.Equal(bookRequest.AuthorId, resultValue.Book.AuthorId);
            Assert.Equal(bookRequest.Quantity, resultValue.Book.Quantity);
            Assert.Equal(bookRequest.Price, resultValue.Book.Price);
            Assert.Equal(bookRequest.LastUpdated, resultValue.Book.LastUpdated);
        }
    }
}
