using Books.Entities;
using Books.Services;
using Books.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Books.Controllers
{
    [Route("api")]
    public class BooksController : Controller
    {
        private readonly IBooksData _booksData;

        public BooksController(IBooksData booksData)
        {
            _booksData = booksData;
        }

        // Task 1: GET-Routes

        [Route("books")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _booksData.GetAllBooks();
            if (books == null) {
                return NotFound();
            }

            // Use BooksListViewModel as DTO to return only certain fields
            var booksResponse = books.Select(book => {
                
                var selectedAuthors = book.Authors
                    .Select(x => new AuthorsListViewModel() 
                    { 
                        Id = x.Id, 
                        Name = x.Name
                    }).ToList();
                
                return new BooksListViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    Authors = selectedAuthors,
                    Publisher = new PublisherViewModel() 
                    { 
                        Id = book.Publisher.Id, 
                        Name = book.Publisher.Name 
                    }    
                }; 
            });

            return Ok(booksResponse);
        }

        [Route("books/{bookId}")]
        [HttpGet]
        public IActionResult GetBookById(int bookId)
        {
            Book book = _booksData.GetBookById(bookId);
            return Ok(book);
        }

        [Route("books/{bookId}/authors")]
        [HttpGet]
        public IActionResult GetAuthorsByBookId(int bookId)
        {
            var authors = _booksData.GetAuthorsByBookId(bookId);
            if (authors == null) {
                return NotFound();
            }

            var authorsResponse = authors.Select(author =>
                new AuthorsListViewModel()
                {
                    Id = author.Id,
                    Name = author.Name,
                });
            return Ok(authorsResponse);

        }

        // Task 2: POST-routes
    }
}
