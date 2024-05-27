namespace BooksMVC.ViewModels
{
    public class BooksByPublisherViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
    }
}
