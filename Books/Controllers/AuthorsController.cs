using BooksMVC.ViewModels;
using Lib.Entities;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksMVC.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IBooksRepository _booksData;

        public AuthorsController(IBooksRepository booksData)
        {
            _booksData = booksData;
        }

        // Task 1: GET-Routes

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _booksData.GetAllAuthors();
            if (authors is null) {
                return NotFound();
            }

            // Use AuthorsListViewModel as DTO to return only certain fields
            var authorsResponse = authors.Select(author =>
                new AuthorViewModel()
                {
                    Id = author.Id,
                    Name = author.Name,
                });

            return Ok(authorsResponse);
        }

        [Route("{authorId}")]
        [HttpGet]
        public IActionResult GetAuthorById(int authorId)
        {
            var author = _booksData.GetAuthorById(authorId);
            if (author is null) {
                return NotFound();
            }

            var authorViewModel = new AuthorViewModel()
            {
                Id = author.Id,
                Name = author.Name
            };

            return Ok(authorViewModel);
        }

        [Route("{authorId}/books")]
        [HttpGet]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var books = _booksData.GetBooksByAuthorId(authorId);
            if (books is null) {
                return NotFound();
            }

            var booksResponse = books.Select(book => {

                return new BooksByAuthorViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
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

        [Route("")]
        [HttpPost]
        public IActionResult Add([FromBody] AuthorCreateViewModel createAuthorViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var newAuthor = new Author()
            {
                Name = createAuthorViewModel.Name

            };

            _booksData.AddAuthor(newAuthor);

            return CreatedAtAction(nameof(Add), newAuthor);

        }

        // Task 3: UPDATE-routes

        [Route("{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] AuthorUpdateViewModel updateAuthorViewModel)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var foundAuthor = _booksData.GetAuthorById(id);

            if (foundAuthor is null) {
                return NotFound();
            }

            foundAuthor.Name = updateAuthorViewModel.Name;
            _booksData.UpdateAuthor(foundAuthor);

            return NoContent();
        }


        [Route("assign-book")]
        [HttpPut]
        public IActionResult AssignBookToAuthor([FromBody] AssignBookToAuthorViewModel assignBookToAuthorViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _booksData.GetBookById(assignBookToAuthorViewModel.BookId);
            var author = _booksData.GetAuthorById(assignBookToAuthorViewModel.AuthorId);

            if (author is null || book is null) {
                return NotFound();
            }

            _booksData.AssignBookToAuthor(book, author);

            return NoContent();
        }

        // Task 3: DELETE-routes

        [Route("")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var foundAuthor = _booksData.GetAuthorById(id);

            if (foundAuthor is null) {
                return NotFound();
            }

            _booksData.DeleteAuthor(foundAuthor);
            return NoContent();
        }
    }
}
