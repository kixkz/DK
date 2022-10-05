namespace BookStore.Models.Requests
{
    public class AddPersonRequest
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
