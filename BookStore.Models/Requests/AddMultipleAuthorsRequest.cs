namespace BookStore.Models.Requests
{
    public class AddMultipleAuthorsRequest
    {
        public IEnumerable<AddAuthorRequest> AuthorRequest { get; set; }

        public string Reason { get; set; }
    }
}
