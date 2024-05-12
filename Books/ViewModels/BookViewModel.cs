using Books.Entities;

namespace Books.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public List<AuthorViewModel>? Authors { get; set; }
        public PublisherViewModel? Publisher { get; set; }
    }
}
