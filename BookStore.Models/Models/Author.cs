namespace BookStore.Models.Models
{
    public record Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string NickName { get; set; }
    }
}
