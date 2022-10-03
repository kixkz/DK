namespace BookStore.Models.Requests
{
    public class AddAuthorRequest
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string NickName { get; set; }
    }
}
