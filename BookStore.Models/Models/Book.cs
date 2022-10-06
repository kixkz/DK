namespace BookStore.Models.Models
{
    public record Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int AuthorId { get; set; }

        public int Quantity  { get; set; }

        public DateTime LastUpdated { get; set; }

        public decimal Price { get; set; }

    }
}
