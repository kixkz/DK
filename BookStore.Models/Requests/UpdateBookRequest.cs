namespace BookStore.Models.Requests
{
    public class UpdateBookRequest : AddBookRequest
    {
        public int Id { get; set; }
    }
}
