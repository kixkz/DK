using MessagePack;

namespace BookStore.Models.Models
{
    [MessagePackObject]
    public record Person
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public int Age { get; set; }
        [Key(3)]
        public DateTime DateOfBirth { get; set; }
    }
}