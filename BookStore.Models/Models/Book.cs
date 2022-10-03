﻿namespace BookStore.Models.Models
{
    public record Book
    {
        public int Id { get; set; }

        public string Title { get; init; }

        public int AuthorId { get; init; }

    }
}
