namespace Books.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public List<Author> Authors { get; set; }
        public Publisher Publisher { get; set; }

    }
}
