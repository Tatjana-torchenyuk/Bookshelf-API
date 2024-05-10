using Books.Entities;
using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels
{
    public class CreateBookViewModel
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required, StringLength(15)]
        public string Isbn { get; set; }

        [Required]
        public List<CreateAuthorViewModel> Authors { get; set; }

        [Required]
        public CreatePublisherViewModel Publisher { get; set; }
    }
}
