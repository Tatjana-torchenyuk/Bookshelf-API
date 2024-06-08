using BooksMVC.ViewModels;
using Lib.Entities;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksMVC.Controllers
{
    [Route("api")]
    public class AuthorsController : Controller
    {
        private readonly IBooksRepository _booksData;

        public AuthorsController(IBooksRepository booksData)
        {
            _booksData = booksData;
        }

        // GET-Routes

        [Route("authors")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _booksData.GetAllAuthors();
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

        [Route("authors/{id}")]
        [HttpGet]
        public IActionResult GetAuthorById(int id)
        {
            var author = _booksData.GetAuthorById(id);
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

        [Route("authors/{id}/books")]
        [HttpGet]
        public IActionResult GetBooksByAuthorId(int id)
        {
            var author = _booksData.GetAuthorById(id);
            if (author is null) {
                return NotFound();
            }

            var books = _booksData.GetBooksByAuthorId(id);
            if (books is null) {
                return NotFound();
            }

            var booksResponse = books.Select(book => {

                return new BooksByAuthorViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    Publisher = book.Publisher != null ? new PublisherViewModel()
                    {
                        Id = book.Publisher.Id,
                        Name = book.Publisher.Name
                    } : null
                };
            });

            return Ok(booksResponse);

        }

        // POST-routes

        [Route("authors")]
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

            return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, newAuthor);

        }

        // UPDATE-routes

        [Route("authors/{id}")]
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


        [Route("authors/assign-to-book")]
        [HttpPut]
        public IActionResult AssignBookToAuthor([FromBody] AuthorToBookUpdateViewModel assignBookToAuthorViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _booksData.GetBookById(assignBookToAuthorViewModel.BookId);
            var author = _booksData.GetAuthorById(assignBookToAuthorViewModel.AuthorId);

            if (author is null || book is null) {
                return NotFound();
            }

            _booksData.UpdateAuthorToBook(book, author);

            return NoContent();
        }

        // DELETE-routes

        [Route("authors")]
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
