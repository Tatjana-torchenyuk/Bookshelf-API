namespace Books.ViewModels
{
    public class BooksByAuthorViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public PublisherViewModel Publisher { get; set; }
    }
}
