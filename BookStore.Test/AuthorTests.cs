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
    public class AuthorTests
    {
        private IList<Author> _authors = new List<Author>()
        {
            new Author()
            {
                Id = 1,
                Age = 74,
                DateOfBirth = DateTime.Now,
                Name = "Author Name",
                NickName = "Nickname"
            },
            new Author()
            {
                Id = 2,
                Age = 74,
                DateOfBirth = DateTime.Now,
                Name = "Another Name",
                NickName = "Another Nickname"
            }
        };

        private readonly IMapper _mapper;
        private Mock<ILogger<AuthorService>> _loggerMock;
        private Mock<ILogger<AuthorController>> _loggerAuthorControllerMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;

        public AuthorTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });

            _mapper = mockMapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<AuthorService>>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerAuthorControllerMock = new Mock<ILogger<AuthorController>>();
        }

        [Fact]
        public async Task Author_GetAll_Count_Check()
        {
            //setup
            var expectedCount = 2;

            _authorRepositoryMock.Setup(x => x.GetAllAuthors()).ReturnsAsync(_authors);

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            var result = await controller.Get();

            var okObjectResult = result as OkObjectResult;

            var authors = okObjectResult.Value as IEnumerable<Author>;
            Assert.NotNull(authors);
            Assert.NotEmpty(authors);
            Assert.Equal(expectedCount, authors.Count());
            Assert.Equal(authors, _authors);
        }

        [Fact]
        public async Task Author_GetAythorById_Ok()
        {
            //setup
            var authorId = 1;
            var expectedAuthor = _authors.First(x => x.Id == authorId);

            _authorRepositoryMock.Setup(x => x.GetByID(authorId)).ReturnsAsync(expectedAuthor);

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.GetAuthorById(authorId);

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var author = okObjectResult.Value as Author;
            Assert.NotNull(author);
            Assert.Equal(authorId, author.Id);
        }

        [Fact]
        public async Task Author_GetAuthorById_NotFound()
        {
            //setup
            var authorId = 3;
            var expectedAuthor = _authors.FirstOrDefault(x => x.Id == authorId);

            _authorRepositoryMock.Setup(x => x.GetByID(authorId)).ReturnsAsync(expectedAuthor);

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.GetAuthorById(authorId);

            //Assert
            var notObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notObjectResult);

            var returnedAuthorId = (int)notObjectResult.Value;
            Assert.NotNull(returnedAuthorId);
            Assert.Equal(authorId, returnedAuthorId);
        }

        [Fact]
        public async Task AddAuthorOk()
        {
            //setup
            var authorRequest = new AddAuthorRequest()
            {
                NickName = "New nickname",
                Age = 22,
                DateOfBirth = DateTime.Now,
                Name = "Test Author Name"
            };

            var exxpectedAuthorId = 3;

            _authorRepositoryMock.Setup(x => x.AddAuthor(It.IsAny<Author>())).Callback(() =>
            {
                _authors.Add(new Author()
                {
                    Id = exxpectedAuthorId,
                    Name = authorRequest.NickName,
                    Age = authorRequest.Age,
                    DateOfBirth = authorRequest.DateOfBirth,
                    NickName = authorRequest.NickName,
                });
            }).ReturnsAsync(() => _authors.FirstOrDefault(x => x.Id == exxpectedAuthorId));

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.AddAuthor(authorRequest);

            //asset
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as AddAuthorResponse;
            Assert.NotNull(resultValue);
            Assert.Equal(exxpectedAuthorId, resultValue.Author.Id);
        }

        [Fact]
        public async Task Author_AddAuthorWhenExist()
        {
            //setup
            var authorRequest = new AddAuthorRequest()
            {
                Age = 74,
                DateOfBirth = DateTime.Now,
                Name = "Author Name",
                NickName = "Nickname"
            };

            _authorRepositoryMock.Setup(x => x.GetAuthorByName(authorRequest.Name))
                .ReturnsAsync(_authors.FirstOrDefault(x => x.Name == authorRequest.Name));

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.AddAuthor(authorRequest);

            //asset
            var badObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badObjectResult);

            var resultValue = badObjectResult.Value as AddAuthorResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Author already exist", resultValue.Message);
        }

        [Fact]
        public async Task Author_DeleteOk()
        {
            //setup
            var authorId = 1;

            var authorToDelete = _authors.FirstOrDefault(x => x.Id == authorId);

            _authorRepositoryMock.Setup(x => x.DeleteAuthor(authorId)).Callback( () =>
            {
                _authors.Remove(_authors.FirstOrDefault(x =>x.Id == authorId));
            }).ReturnsAsync(() => authorToDelete);

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.Delete(authorId);

            var okObjectResult = result as OkObjectResult;

            //asset
            Assert.NotNull(okObjectResult);
            Assert.Equal(1, _authors.Count);

            var resultValue = okObjectResult.Value as Author;

            Assert.NotNull(resultValue);
            Assert.Equal(authorToDelete, resultValue);
        }

        [Fact]
        public async Task Author_DeleteBadRequest()
        {
            //setup
            var authorId = 3;

            var authorToDelete = _authors.FirstOrDefault(x => x.Id == authorId);

            _authorRepositoryMock.Setup(x => x.DeleteAuthor(authorId)).ReturnsAsync(() => authorToDelete);

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.Delete(authorId);

            var badRequestObjectResult = result as BadRequestObjectResult;

            //asset
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(2, _authors.Count);

            var resultValue = badRequestObjectResult.Value as Author;

            Assert.Null(resultValue);
            Assert.Equal(authorToDelete, resultValue);
        }

        [Fact]
        public async Task UpdateAuthorOk()
        {
            //setup
            var authorRequest = new UpdateAuthorRequest()
            {
                Id = 1,
                Age = 74,
                DateOfBirth = DateTime.Now,
                Name = "Author Name",
                NickName = "New Nickname"
            };


            _authorRepositoryMock.Setup(x => x.GetAuthorByName(authorRequest.Name)).ReturnsAsync(new Author());
            _authorRepositoryMock.Setup(x => x.UpdateAuthor(It.IsAny<Author>())).Callback(() =>
            {
                var author = _authors.FirstOrDefault(x => x.Id == authorRequest.Id);
                author.DateOfBirth = authorRequest.DateOfBirth;
                author.Name = authorRequest.Name;
                author.NickName = authorRequest.NickName;
                author.Age = authorRequest.Age;
            }).ReturnsAsync(() => _authors.FirstOrDefault(x => x.Id == authorRequest.Id));

            //inject
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _loggerMock.Object, _bookRepositoryMock.Object);

            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service, _mapper);

            //act
            var result = await controller.Update(authorRequest);

            //asset
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as AddAuthorResponse;

            Assert.NotNull(resultValue);
            Assert.Equal(authorRequest.DateOfBirth, resultValue.Author.DateOfBirth);
            Assert.Equal(authorRequest.Name, resultValue.Author.Name);
            Assert.Equal(authorRequest.Age, resultValue.Author.Age);
            Assert.Equal(authorRequest.NickName, resultValue.Author.NickName);
        }
    }
}