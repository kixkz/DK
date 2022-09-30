using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Models;

namespace BookStore.BL.Interfaces
{
    public interface IAuthorService
    {
        Author AddAuthor(Author author);

        Author? DeleteAuthor(int authorId);

        IEnumerable<Author> GetAllAuthors();

        Author? GetByID(int id);

        Author UpdateAuthor(Author author);
    }
}
