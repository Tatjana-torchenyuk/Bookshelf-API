using Books.Entities;
using Books.Services;
using Books.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [Route("api")]
    public class AuthorsController : Controller
    {
        private readonly IBooksData _booksData;

        public AuthorsController(IBooksData booksData)
        {
            _booksData = booksData;
        }

        // Task 1: GET-Routes

        [Route("authors")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _booksData.GetAllAuthors();
            if (authors == null) {
                return NotFound();
            }

            // Use AuthorsListViewModel as DTO to return only certain fields
            var authorsResponse = authors.Select(author =>
                new AuthorsListViewModel()
                {
                    Id = author.Id,
                    Name = author.Name,
                });

            return Ok(authorsResponse);
        }

        [Route("authors/{authorId}")]
        [HttpGet]
        public IActionResult GetAuthorById(int authorId)
        {
            var author = _booksData.GetAuthorById(authorId);
            if (author == null) {
                return NotFound();
            }

            var selectedAuthor = new AuthorsListViewModel()
            {
                Id = author.Id,
                Name = author.Name
            };

            return Ok(selectedAuthor);
        }

        [Route("authors/{authorId}/books")]
        [HttpGet]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var books = _booksData.GetBooksByAuthorId(authorId);
            if (books == null) {
                return NotFound();
            }

            var booksResponse = books.Select(book => {

                return new BooksByAuthorViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    Publisher = new PublisherViewModel()
                    {
                        Id = book.Publisher.Id,
                        Name = book.Publisher.Name
                    }
                };
            });

            return Ok(booksResponse);

        }

        // Task 2: POST-routes

        [Route("authors")]
        [HttpPost]
        public IActionResult Add([FromBody]CreateAuthorViewModel createAuthorViewModel)
        {
            if ( !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newAuthor = new Author()
            {
                Name = createAuthorViewModel.Name            
            };

            _booksData.Add(newAuthor);

            return CreatedAtAction(nameof(Add), newAuthor);

        }
    }
}
