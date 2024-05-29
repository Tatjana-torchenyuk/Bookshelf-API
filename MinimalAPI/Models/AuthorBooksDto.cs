namespace MinimalAPI.Models
{
    public class AuthorBooksDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
    }
}
