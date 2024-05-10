using Books.Entities;

namespace Books.Services
{
    public interface IBooksData
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int bookId);
        IEnumerable<Author> GetAllAuthors();
        Author GetAuthorById(int authorId);
        IEnumerable<Publisher> GetAllPublishers();
        Publisher GetPublisherById(int publisherId);
        IEnumerable<Book> GetBooksByAuthorId(int authorId);
        IEnumerable<Author> GetAuthorsByBookId(int bookId);
        Publisher GetPublisherByBookId(int bookId);
        void Add(Book book);
        void Add(Author author);
        void Add(Publisher publisher);


    }
}
