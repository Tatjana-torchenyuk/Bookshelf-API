using Lib.Entities;

namespace Books.Services
{
    public interface IBooksRepository
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
        void AddBook(Book book);
        void AddAuthor(Author author);
        void AddPublisher(Publisher publisher);
        void UpdateBook(Book book);
        void UpdateAuthor(Author author);
        void UpdatePublisher(Publisher publisher);
        void AssignBookToAuthor(Book book, Author author);
        void AssignBookToPublisher(Book book, Publisher publisher);
        void DeleteBook(Book book);
        void DeleteAuthor(Author author);
        void DeletePublisher(Publisher publisher);


    }
}
