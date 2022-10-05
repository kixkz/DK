using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.InMemotyRepositories
{
    public class AuthorInMemoryRepository 
    {
        private static List<Author> _authors = new List<Author>()
        {
            new Author()
            {
                Id = 1,
                Name = "Toshko",
                DateOfBirth = DateTime.Now,
                NickName = "Tosheto"
            },
            new Author()
            {
                Id = 2,
                Name = "Pesho",
                DateOfBirth = DateTime.Now,
                NickName = "Peshko"
            },
            new Author()
            {
                Id = 3,
                Name = "Jeka",
                DateOfBirth = DateTime.Now,
                NickName = "Jekata"
            }
        };

        private readonly ILogger<AuthorInMemoryRepository> _logger;

        public AuthorInMemoryRepository()
        {
            _authors = new List<Author>();
        }

        public Author AddAuthor(Author author)
        {
            try
            {
                if (GetAuthorByName(author.Name) == null)
                {
                    _authors.Add(author);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }

            return author;
        }

        public Author? DeleteAuthor(int authorId)
        {
            if (authorId < 0) return null;

            var author = _authors.FirstOrDefault(x => x.Id == authorId);

            _authors.Remove(author);

            if (author != null) return author;

            return author;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _authors;
        }

        public Author? GetByID(int id)
        {
            return _authors.FirstOrDefault(x => x.Id == id);
        }

        public Author UpdateAuthor(Author author)
        {
            var existingAuthor = _authors.FirstOrDefault(x => x.Id == author.Id);
            if (existingAuthor == null) return null;

            _authors.Remove(existingAuthor);

            _authors.Add(author);

            return author;
        }

        public Author? GetAuthorByName(string authorName)
        {
            return _authors.FirstOrDefault(x => x.Name.Equals(authorName));
        }

        public bool AddMultipleAuthors(IEnumerable<Author> authosCollection)
        {
            try
            {
                _authors.AddRange(authosCollection);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Unable to add multiple authors with message {ex.Message}");
                return false;
            }


        }
    }
}
