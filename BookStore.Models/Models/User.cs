namespace BookStore.Models.Models
{
    public record User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}