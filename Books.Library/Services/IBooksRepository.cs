using Lib.Entities;

namespace Lib.Services
{
    public interface IBooksRepository
    {
        // Author data

        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int bookId);
        IEnumerable<Author> GetAuthorsByBookId(int bookId);
        void AddBook(Book book);
        void UpdateAuthor(Author author);
        void AssignBookToAuthor(Book book, Author author);
        void DeleteAuthor(Author author);

        // Book data

        IEnumerable<Author> GetAllAuthors();
        Author GetAuthorById(int authorId);
        IEnumerable<Book> GetBooksByAuthorId(int authorId);
        void AddAuthor(Author author);
        void UpdateBook(Book book);
        void DeleteBook(Book book);

        // Publisher data

        IEnumerable<Publisher> GetAllPublishers();
        Publisher GetPublisherById(int publisherId);
        IEnumerable<Book> GetBooksByPublisherId(int publisherId);
        void AddPublisher(Publisher publisher);
        void UpdatePublisher(Publisher publisher);
        void AssignBookToPublisher(Book book, Publisher publisher);
        void DeletePublisher(Publisher publisher);


    }
}
