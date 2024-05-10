using Books.Entities;

namespace Books.ViewModels
{
    public class BooksListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public List<AuthorsListViewModel> Authors { get; set; }
        public PublisherViewModel Publisher { get; set; }
    }
}
