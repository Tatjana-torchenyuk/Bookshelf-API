
using Lib.DbContexts;
using Lib.Entities;

namespace Books.Services
{
    public class EfBooksRepository : IBooksRepository
    {
        private readonly BookDbContext _context;
        public EfBooksRepository(BookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetBookById(int bookId)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == bookId);
            if (book != null) {
                return book;
            }
            return null;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors;
        }

        public Author GetAuthorById(int authorId)
        {
            var author = _context.Authors.FirstOrDefault(x => x.Id == authorId);
            if (author != null) {
                return author;
            }
            return null;
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return _context.Publishers;
        }

        public Publisher GetPublisherById(int publisherId)
        {
            var publisher = _context.Publishers.FirstOrDefault(x => x.Id == publisherId);
            if (publisher != null) {
                return publisher;
            }
            return null;
        }

        public IEnumerable<Book> GetBooksByAuthorId(int authorId)
        {
            return _context.Books.Where(book => book.Authors.Any(author => author.Id == authorId));
        }

        public IEnumerable<Author> GetAuthorsByBookId(int bookId)
        {
            return _context.Authors.Where(author => author.Books.Any(book => book.Id == bookId));

        }

        public Publisher GetPublisherByBookId(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book != null) {
                return book.Publisher;
            }
            return null;
        }

        // CREATE

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
        }

        // UPDATE

        public void UpdateAuthor(Author author)
        {
            var updatedAuthor = GetAuthorById(author.Id);
            if (updatedAuthor != null) {
                updatedAuthor.Name = author.Name;
            }
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            var updatedBook = GetBookById(book.Id);
            if (updatedBook != null) {
                updatedBook.Title = book.Title;
                updatedBook.ISBN = book.ISBN;
                updatedBook.Publisher = book.Publisher;
            }
            _context.SaveChanges();
        }

        public void UpdatePublisher(Publisher publisher)
        {
            var updatedPublisher = GetPublisherById(publisher.Id);
            if (updatedPublisher != null) {
                updatedPublisher.Name = publisher.Name;
            }
            _context.SaveChanges();
        }

        public void AssignBookToAuthor(Book book, Author author)
        {
            book.Authors.Add(author);
            author.Books.Add(book);

            _context.SaveChanges();
        }

        public void AssignBookToPublisher(Book book, Publisher publisher)
        {
            book.Publisher = publisher;
            publisher.Books.Add(book);
            _context.SaveChanges();
        }

        // DELETE

        public void DeleteBook(Book book)
        {
            // find the author(s) that wrote the to-be-deleted book
            var authors = _context.Authors.Where(author => author.Books.Contains(book));

            // remove book from the author(s) book list
            foreach (var author in authors) {
                author.Books.Remove(book);
            }

            // find the publisher of the to-be-deleted book
            var publishers = _context.Publishers.Where(publisher => publisher.Books.Contains(book));

            // remove book from the publisher book list
            foreach (var publisher in publishers) {
                publisher.Books.Remove(book);
            }

            // remove the book from the list of books
            _context.Books.Remove(book);

            _context.SaveChanges();
        }

        public void DeleteAuthor(Author author)
        {
            // find all the books written by the to-be-deleted author
            var booksWrittenByAuthor = _context.Books.Where(book => book.Authors.Contains(author));

            // remove author from the book(s) author list
            foreach (var book in booksWrittenByAuthor) {
                book.Authors.Remove(author);
            }

            // remove the author from the list of authors
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }

        public void DeletePublisher(Publisher publisher)
        {
            // find all the books linked to the to-be-deleted publisher
            var booksWithPublisher = _context.Books.Where(book => book.Publisher == publisher);

            // remove publisher from the book(s) publisher list
            foreach (var book in booksWithPublisher) {
                book.Publisher = null;
            }

            //remove the publisher from the list of publishers
            _context.Publishers.Remove(publisher);
            _context.SaveChanges();
        }
    }
}

