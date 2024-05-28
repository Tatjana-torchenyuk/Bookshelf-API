using Lib.Entities;

namespace Lib.Services
{
    public class InMemoryBooksRepository : IBooksRepository
    {
        private static List<Book> _books = new List<Book>();
        private static List<Author> _authors = new List<Author>();
        private static List<Publisher> _publishers = new List<Publisher>();
        private static int bookIdIncrement = 1;
        private static int authorIdIncrement = 1;
        private static int publisherIdIncrement = 1;

        public InMemoryBooksRepository()
        {
            PopulateData();
        }

        private void PopulateData()
        {
            // Authors
            var patrickRothfuss = new Author { Id = authorIdIncrement++, Name = "Patrick Rothfuss", Books = new List<Book>() };
            var brandonSanderson = new Author { Id = authorIdIncrement++, Name = "Brandon Sanderson", Books = new List<Book>() };
            var georgeRRMartin = new Author { Id = authorIdIncrement++, Name = "George R.R. Martin", Books = new List<Book>() };
            var jRRTolkien = new Author { Id = authorIdIncrement++, Name = "J.R.R. Tolkien", Books = new List<Book>() };
            var jKRowling = new Author { Id = authorIdIncrement++, Name = "J.K. Rowling", Books = new List<Book>() };
            var robertJordan = new Author { Id = authorIdIncrement++, Name = "Robert Jordan", Books = new List<Book>() };

            // Publishers
            var dawBooks = new Publisher { Id = publisherIdIncrement++, Name = "DAW Books", Books = new List<Book>() };
            var torBooks = new Publisher { Id = publisherIdIncrement++, Name = "Tor Books", Books = new List<Book>() };
            var bantamSpectra = new Publisher { Id = publisherIdIncrement++, Name = "Bantam Spectra", Books = new List<Book>() };
            var allenAndUnwin = new Publisher { Id = publisherIdIncrement++, Name = "Allen & Unwin", Books = new List<Book>() };
            var bloomsburyPublishing = new Publisher { Id = publisherIdIncrement++, Name = "Bloomsbury Publishing", Books = new List<Book>() };

            // Books
            var theNameOfTheWind = new Book { Id = bookIdIncrement++, Title = "The Name of the Wind", ISBN = "978-1473211896", Publisher = dawBooks, Authors = new List<Author>() };
            var mistbornTheFinalEmpire = new Book { Id = bookIdIncrement++, Title = "Mistborn: The Final Empire", ISBN = "978-0765377135", Publisher = torBooks, Authors = new List<Author>() };
            var aGameOfThrones = new Book { Id = bookIdIncrement++, Title = "A Game of Thrones", ISBN = "978-0553386790", Publisher = bantamSpectra, Authors = new List<Author>() };
            var theHobbit = new Book { Id = bookIdIncrement++, Title = "The Hobbit", ISBN = "978-0547928227", Publisher = allenAndUnwin, Authors = new List<Author>() };
            var harryPotterAndThePhilosophersStone = new Book { Id = bookIdIncrement++, Title = "Harry Potter and the Philosopher's Stone", ISBN = "978-0590353427", Publisher = bloomsburyPublishing, Authors = new List<Author>() };
            var aMemoryOfLight = new Book { Id = bookIdIncrement++, Title = "A Memory of Light", ISBN = "978-0765325952", Publisher = torBooks, Authors = new List<Author>() };

            // Populate lists
            _books.AddRange(new List<Book> { theNameOfTheWind, mistbornTheFinalEmpire, aGameOfThrones, theHobbit, harryPotterAndThePhilosophersStone, aMemoryOfLight });
            _authors.AddRange(new List<Author>() { patrickRothfuss, brandonSanderson, georgeRRMartin, jRRTolkien, jKRowling, robertJordan });
            _publishers.AddRange(new List<Publisher> { dawBooks, torBooks, bantamSpectra, allenAndUnwin, bloomsburyPublishing });

            // Add relation Books -> Publisher
            dawBooks.Books.Add(theNameOfTheWind);
            torBooks.Books.Add(mistbornTheFinalEmpire);
            torBooks.Books.Add(aMemoryOfLight);
            bantamSpectra.Books.Add(aGameOfThrones);
            allenAndUnwin.Books.Add(theHobbit);
            bloomsburyPublishing.Books.Add(harryPotterAndThePhilosophersStone);

            // Add relation Authors <-> Books
            patrickRothfuss.Books.Add(theNameOfTheWind);
            brandonSanderson.Books.Add(mistbornTheFinalEmpire);
            brandonSanderson.Books.Add(aMemoryOfLight);
            georgeRRMartin.Books.Add(aGameOfThrones);
            jRRTolkien.Books.Add(theHobbit);
            jKRowling.Books.Add(harryPotterAndThePhilosophersStone);
            robertJordan.Books.Add(aMemoryOfLight);

            // Add relation Books <-> Authors
            theNameOfTheWind.Authors.Add(patrickRothfuss);
            mistbornTheFinalEmpire.Authors.Add(brandonSanderson);
            aMemoryOfLight.Authors.Add(brandonSanderson);
            aMemoryOfLight.Authors.Add(robertJordan);
            aGameOfThrones.Authors.Add(georgeRRMartin);
            theHobbit.Authors.Add(jRRTolkien);
            harryPotterAndThePhilosophersStone.Authors.Add(jKRowling);

        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books;
        }

        public Book GetBookById(int bookId)
        {
            var book = _books.FirstOrDefault(x => x.Id == bookId);
            if (book != null) {
                return book;
            }
            return null;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _authors;
        }

        public Author GetAuthorById(int authorId)
        {
            var author = _authors.FirstOrDefault(x => x.Id == authorId);
            if (author != null) {
                return author;
            }
            return null;
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return _publishers;
        }

        public Publisher GetPublisherById(int publisherId)
        {
            var publisher = _publishers.FirstOrDefault(x => x.Id == publisherId);
            if (publisher != null) {
                return publisher;
            }
            return null;
        }

        public IEnumerable<Book> GetBooksByAuthorId(int authorId)
        {
            return _books.Where(book => book.Authors.Any(author => author.Id == authorId));
        }

        public IEnumerable<Author> GetAuthorsByBookId(int bookId)
        {
            return _authors.Where(author => author.Books.Any(book => book.Id == bookId));

        }

        public IEnumerable<Book> GetBooksByPublisherId(int publisherId)
        {
            return _books.Where(book => book.Publisher.Id == publisherId);
        }

        // CREATE

        public void AddBook(Book book)
        {
            if (_books.Count > 0) {
                book.Id = _books.Max(x => x.Id) + 1;
            } else {
                book.Id = 1;
            }

            _books.Add(book);
        }

        public void AddAuthor(Author author)
        {
            if (_authors.Count > 0) {
                author.Id = _authors.Max(x => x.Id) + 1;
            } else {
                author.Id = 1;
            }
            _authors.Add(author);
        }

        public void AddPublisher(Publisher publisher)
        {
            if (_publishers.Count > 0) {
                publisher.Id = _publishers.Max(x => x.Id) + 1;
            } else {
                publisher.Id = 1;
            }
            _publishers.Add(publisher);
        }

        // UPDATE

        public void UpdateAuthor(Author author)
        {
            var updatedAuthor = _authors.FirstOrDefault(x => x.Id == author.Id);
            if (updatedAuthor != null) {
                updatedAuthor.Name = author.Name;
            }
        }

        public void UpdateBook(Book book)
        {
            var updatedBook = _books.FirstOrDefault(x => x.Id == book.Id);
            if (updatedBook != null) {
                updatedBook.Title = book.Title;
                updatedBook.ISBN = book.ISBN;
                updatedBook.Publisher = book.Publisher;
            }
        }

        public void UpdatePublisher(Publisher publisher)
        {
            var updatedPublisher = _publishers.FirstOrDefault(x => x.Id == publisher.Id);
            if (updatedPublisher != null) {
                updatedPublisher.Name = publisher.Name;
            }
        }

        public void AssignBookToAuthor(Book book, Author author)
        {
            book.Authors.Add(author);
            author.Books.Add(book);
        }

        public void AssignBookToPublisher(Book book, Publisher publisher)
        {
            book.Publisher = publisher;
            publisher.Books.Add(book);
        }

        // DELETE

        public void DeleteBook(Book book)
        {
            // find the author(s) that wrote the to-be-deleted book
            var authors = _authors.Where(author => author.Books.Contains(book)).ToList();

            // delete the book from each author's list
            foreach (var author in authors) {
                author.Books.Remove(book);
            }

            // find the publisher of the to-be-deleted book
            var publisher = _publishers.FirstOrDefault(publisher => publisher.Id == book.Publisher.Id);
            
            if (publisher != null) {
                // Delete the book from the publisher's list
                publisher.Books.Remove(book);
            }

            // remove the book from the list of books
            _books.Remove(book);
        }

        public void DeleteAuthor(Author author)
        {
            // find all the books written by the to-be-deleted author
            var books = _books.Where(book => book.Authors.Contains(author)).ToList();

            // delete the author from each book
            foreach (var book in books) {
                book.Authors.Remove(author);
            }

            // remove the author from the list of authors
            _authors.Remove(author);
        }

        public void DeletePublisher(Publisher publisher)
        {
            // find all the books published by the to-be-deleted publisher
            var books = _books.Where(book => book.Publisher == publisher).ToList();

            // set publisher reference to null
            foreach (var book in books) {
                book.Publisher = null;
            }

            //remove the publisher from the list of publishers
            _publishers.Remove(publisher);
        }
    }
}
