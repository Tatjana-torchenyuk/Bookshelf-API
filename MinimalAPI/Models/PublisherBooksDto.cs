namespace MinimalAPI.Models
{
    public class PublisherBooksDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public List<AuthorDto>? Authors { get; set; }
    }
}
