using Books.Entities;

namespace Books.Services
{
    public class EfBooksRepository : IBooksRepository
    {
        public void AddAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void AddBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void AddPublisher(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public void AssignAuthorToBook(Author author, Book book)
        {
            throw new NotImplementedException();
        }

        public void AssignBookToAuthor(Book book, Author author)
        {
            throw new NotImplementedException();
        }

        public void AssignBookToPublisher(Book book, Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void DeletePublisher(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            throw new NotImplementedException();
        }

        public Author GetAuthorById(int authorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Author> GetAuthorsByBookId(int bookId)
        {
            throw new NotImplementedException();
        }

        public Book GetBookById(int bookId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetBooksByAuthorId(int authorId)
        {
            throw new NotImplementedException();
        }

        public Publisher GetPublisherByBookId(int bookId)
        {
            throw new NotImplementedException();
        }

        public Publisher GetPublisherById(int publisherId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void UpdatePublisher(Publisher publisher)
        {
            throw new NotImplementedException();
        }
    }
}
