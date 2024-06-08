using BooksMVC.ViewModels;
using Lib.Entities;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksMVC.Controllers
{
    [Route("api")]
    public class PublishersController : Controller
    {
        private readonly IBooksRepository _booksData;

        public PublishersController(IBooksRepository booksData)
        {
            _booksData = booksData;
        }

        // GET-routes

        [Route("publishers")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var publishers = _booksData.GetAllPublishers();
            if (publishers is null) {
                return NotFound();
            }

            var publisherResponse = publishers.Select(publisher =>
                new PublisherViewModel()
                {
                    Id = publisher.Id,
                    Name = publisher.Name
                });
            return Ok(publisherResponse);
        }

        [Route("publishers/{id}")]
        [HttpGet]
        public IActionResult GetPublisherById(int id)
        {
            var publisher = _booksData.GetPublisherById(id);
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

        [Route("publishers/{id}/books")]
        [HttpGet]
        public IActionResult GetBooksByPublisherId(int id)
        {
            var publisher = _booksData.GetPublisherById(id);
            if (publisher is null) {
                return NotFound();
            }

            var books = _booksData.GetBooksByPublisherId(id);
            if (books is null) {
                return NotFound();
            }

            var booksResponse = books.Select(book =>
                new BooksByPublisherViewModel()
                {
                    Id = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    Authors = book.Authors.Select(x => new AuthorViewModel() { Id = x.Id, Name = x.Name }).ToList()
                });
            return Ok(booksResponse);

        }

        // POST-routes

        [Route("publishers")]
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

            return CreatedAtAction(nameof(GetPublisherById), new {id = newPublisher.Id }, newPublisher);

        }

        // UPDATE-routes

        [Route("publishers/{id}")]
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

        [Route("publishers/assign-to-book")]
        [HttpPut]
        public IActionResult AssignBookToPublisher([FromBody] PublisherToBookUpdateViewModel assignBookToPublisherViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _booksData.GetBookById(assignBookToPublisherViewModel.BookId);
            var publisher = _booksData.GetPublisherById(assignBookToPublisherViewModel.PublisherId);

            if (book is null || publisher is null) {
                return NotFound();
            }

            _booksData.UpdatePublisherToBook(book, publisher);

            return NoContent();
        }

        // DELETE-routes

        [Route("publishers")]
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
