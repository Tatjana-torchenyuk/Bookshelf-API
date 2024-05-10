using Books.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Books.Services
{
    public class InMemoryData : IBooksData
    {
        private static List<Book> _books = new List<Book>();
        private static List<Author> _authors = new List<Author>();
        private static List<Publisher> _publishers = new List<Publisher>();
        private static int bookIdIncrement = 1;
        private static int authorIdIncrement = 1;
        private static int publisherIdIncrement = 1;

        public InMemoryData()
        {
            this.PopulateData();
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
            var theNameOfTheWind = new Book { Id = bookIdIncrement++, Title = "The Name of the Wind", Isbn = "978-1473211896", Publisher = dawBooks, Authors = new List<Author>() };
            var mistbornTheFinalEmpire = new Book { Id = bookIdIncrement++, Title = "Mistborn: The Final Empire", Isbn = "978-0765377135", Publisher = torBooks, Authors = new List<Author>() };
            var aGameOfThrones = new Book { Id = bookIdIncrement++, Title = "A Game of Thrones", Isbn = "978-0553386790", Publisher = bantamSpectra, Authors = new List<Author>() };
            var theHobbit = new Book { Id = bookIdIncrement++, Title = "The Hobbit", Isbn = "978-0547928227", Publisher = allenAndUnwin, Authors = new List<Author>() };
            var harryPotterAndThePhilosophersStone = new Book { Id = bookIdIncrement++, Title = "Harry Potter and the Philosopher's Stone", Isbn = "978-0590353427", Publisher = bloomsburyPublishing, Authors = new List<Author>() };
            var aMemoryOfLight = new Book { Id = bookIdIncrement++, Title = "A Memory of Light", Isbn = "978-0765325952", Publisher = torBooks, Authors = new List<Author>() };

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

        public Publisher GetPublisherByBookId(int bookId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book != null) {
                return book.Publisher;
            }
            return null;
        }

        public void Add(Book book)
        {
            if (_books.Count > 0) {
                book.Id = _books.Max(x => x.Id) + 1;
            } else {
                book.Id = 1;
            }

            _books.Add(book);
        }

        public void Add(Author author)
        {
            if (_authors.Count > 0) {
                author.Id = _authors.Max(x => x.Id) + 1;
            } else {
                author.Id = 1;
            }
            _authors.Add(author);
        }

        public void Add(Publisher publisher)
        {
            if (_publishers.Count > 0) {
                publisher.Id = _publishers.Max(x => x.Id) + 1;
            } else {
                publisher.Id = 1;
            }
            _publishers.Add(publisher);
        }
    }
}
