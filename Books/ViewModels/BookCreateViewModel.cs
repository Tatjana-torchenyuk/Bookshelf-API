using System.ComponentModel.DataAnnotations;

namespace BooksMVC.ViewModels
{
    public class BookCreateViewModel
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required, StringLength(15)]
        public string ISBN { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int PublisherId { get; set; }

    }
}
