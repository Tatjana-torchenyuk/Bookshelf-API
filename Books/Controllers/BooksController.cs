using BooksMVC.ViewModels;
using Lib.Entities;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;


namespace BooksMVC.Controllers
{
    [Route("api")]
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksData;

        public BooksController(IBooksRepository booksData)
        {
            _booksData = booksData;
        }

        // GET-Routes

        [Route("books")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _booksData.GetAllBooks();
            if (books is null) {
                return NotFound();
            }

            var booksResponse = books.Select(book => {

                var authorsViewModel = book.Authors
                    .Select(x => new AuthorViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

                var publisherViewModel = book.Publisher != null ? new PublisherViewModel()
                {
                    Id = book.Publisher.Id,
                    Name = book.Publisher.Name
                } : null;

                return new BookViewModel()
                {
                    Id = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    Authors = authorsViewModel,
                    Publisher = publisherViewModel
                };
            });

            return Ok(booksResponse);
        }

        [Route("books/{id}")]
        [HttpGet]
        public IActionResult GetBookById(int id)
        {
            var book = _booksData.GetBookById(id);

            if (book is null) {
                return NotFound();
            }

            var bookViewModel = new BookViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Authors = book.Authors.Select(x => new AuthorViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Publisher = new PublisherViewModel
                {
                    Id = book.Publisher.Id,
                    Name = book.Publisher.Name
                }
            };

            return Ok(bookViewModel);
        }

        [Route("books/{id}/authors")]
        [HttpGet]
        public IActionResult GetAuthorsByBookId(int id)
        {
            var authors = _booksData.GetAuthorsByBookId(id);
            if (authors is null) {
                return NotFound();
            }

            var authorsResponse = authors.Select(author =>
                new AuthorViewModel()
                {
                    Id = author.Id,
                    Name = author.Name,
                });
            return Ok(authorsResponse);

        }

        // POST-routes

        [Route("books")]
        [HttpPost]
        public IActionResult Add([FromBody] BookCreateViewModel createBookViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var foundAuthor = _booksData.GetAuthorById(createBookViewModel.AuthorId);
            var foundPublisher = _booksData.GetPublisherById(createBookViewModel.PublisherId);

            var newBook = new Book()
            {
                Title = createBookViewModel.Title,
                ISBN = createBookViewModel.ISBN,
                Authors = new List<Author>() { new Author () {
                    Id = foundAuthor.Id, Name = foundAuthor.Name, Books = foundAuthor.Books}
                },
                Publisher = new Publisher() { Id = foundPublisher.Id, Name = foundPublisher.Name, Books = foundPublisher.Books }
            };


            _booksData.AddBook(newBook);

            var newBookCreated = new BookViewModel() { Id = newBook.Id, Title = newBook.Title };

            return CreatedAtAction(nameof(GetBookById), new {id = newBookCreated.Id}, newBookCreated);
        }

        // UPDATE-routes

        [Route("books/{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] BookUpdateViewModel updateBookViewModel)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var foundBook = _booksData.GetBookById(id);

            if (foundBook is null) {
                return NotFound();
            }

            foundBook.Title = updateBookViewModel.Title;
            foundBook.ISBN = updateBookViewModel.ISBN;

            _booksData.UpdateBook(foundBook);

            return NoContent();
        }

        // DELETE-routes

        [Route("books")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var foundBook = _booksData.GetBookById(id);

            if (foundBook is null) {
                return NotFound();
            }

            _booksData.DeleteBook(foundBook);
            return NoContent();
        }
    }
}
