using Books.Entities;
using Books.Services;
using Books.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [Route("api")]
    public class PublishersController : Controller
    {
        private readonly IBooksData _booksData;

        public PublishersController(IBooksData booksData)
        {
            _booksData = booksData;
        }
        
        // Task 1: GET:routes

        [Route("publishers")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var publishers = _booksData.GetAllPublishers();
            if (publishers == null) {
                return NotFound();
            }

            // Use PublisherViewModel as DTO to return only certain fields
            var publisherResponse = publishers.Select(publisher =>
                new PublisherViewModel()
                {
                    Id = publisher.Id,
                    Name = publisher.Name
                });
            return Ok(publisherResponse);
        }

        [Route("publishers/{publisherId}")]
        [HttpGet]
        public IActionResult GetBookById(int publisherId)
        {
            Publisher publisher = _booksData.GetPublisherById(publisherId);
            return Ok(publisher);
        }

        [Route("publishers/{bookId}/publisher")]
        [HttpGet]
        public IActionResult GetPublisherByBookId(int bookId)
        {
            var publisher = _booksData.GetPublisherByBookId(bookId);
            return Ok(publisher);

        }

        // Task 2: POST:routes

        [Route("publishers/add")]
        [HttpPost]
        public IActionResult Add([FromBody] CreatePublisherViewModel createPublisherViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var newPublisher = new Author()
            {
                Name = createPublisherViewModel.Name,
            /*    Books = createPublisherViewModel.Books,*/

            };
            _booksData.Add(newPublisher);
            return Ok(newPublisher);

        }
    }
}
