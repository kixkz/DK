﻿using BookStore.Models.Models;

namespace BookStore.DL.Repositories.InMemotyRepositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private static List<Author> _authors = new List<Author>();

        public AuthorRepository()
        {

        }

        public Author AddAuthor(Author author)
        {
            try
            {
                _authors.Add(author);
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
    }
}