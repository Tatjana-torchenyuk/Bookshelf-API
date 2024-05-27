using BooksMVC.ViewModels;
using Lib.Entities;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksMVC.Controllers
{
    [Route("api/[controller]")]
    public class PublishersController : Controller
    {
        private readonly IBooksRepository _booksData;

        public PublishersController(IBooksRepository booksData)
        {
            _booksData = booksData;
        }

        // Task 1: GET-routes

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var publishers = _booksData.GetAllPublishers();
            if (publishers is null) {
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

        [Route("{publisherId}")]
        [HttpGet]
        public IActionResult GetBookById(int publisherId)
        {
            var publisher = _booksData.GetPublisherById(publisherId);
            if (publisher is null) {
                return NotFound();
            }

            var publisherViewModel = new PublisherViewModel()
            {
                Id = publisher.Id,
                Name = publisher.Name,
            };

            return Ok(publisherViewModel);
        }

        [Route("{bookId}/publisher")]
        [HttpGet]
        public IActionResult GetPublisherByBookId(int bookId)
        {
            var publisher = _booksData.GetPublisherByBookId(bookId);
            if (publisher is null) {
                return NotFound();
            }

            var publisherViewModel = new PublisherViewModel()
            {
                Id = publisher.Id,
                Name = publisher.Name
            };

            return Ok(publisherViewModel);

        }

        // Task 2: POST-routes

        [Route("")]
        [HttpPost]
        public IActionResult Add([FromBody] PublisherCreateViewModel createPublisherViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var newPublisher = new Publisher()
            {
                Name = createPublisherViewModel.Name
            };

            _booksData.AddPublisher(newPublisher);

            return CreatedAtAction(nameof(Add), newPublisher);

        }

        // Task 3: UPDATE-routes

        [Route("{id}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] PublisherUpdateViewModel updatePublisherViewModel)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var foundPublisher = _booksData.GetPublisherById(id);

            if (foundPublisher is null) {
                return NotFound();
            }

            foundPublisher.Name = updatePublisherViewModel.Name;
            _booksData.UpdatePublisher(foundPublisher);

            return NoContent();
        }

        [Route("assign-book")]
        [HttpPut]
        public IActionResult AssignBookToPublisher([FromBody] AssignBookToPublisherViewModel assignBookToPublisherViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _booksData.GetBookById(assignBookToPublisherViewModel.BookId);
            var publisher = _booksData.GetPublisherById(assignBookToPublisherViewModel.PublisherId);

            if (book is null || publisher is null) {
                return NotFound();
            }

            _booksData.AssignBookToPublisher(book, publisher);

            return NoContent();
        }

        // Task 3: DELETE-routes

        [Route("")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var foundPublisher = _booksData.GetPublisherById(id);

            if (foundPublisher is null) {
                return NotFound();
            }

            _booksData.DeletePublisher(foundPublisher);
            return NoContent();
        }

    }
}
