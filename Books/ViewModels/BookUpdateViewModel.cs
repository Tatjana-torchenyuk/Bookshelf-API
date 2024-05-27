using System.ComponentModel.DataAnnotations;

namespace BooksMVC.ViewModels
{
    public class BookUpdateViewModel
    {
        [Required, StringLength(150)]
        public string Title { get; set; }
        [Required, StringLength(15)]
        public string ISBN { get; set; }
    }
}
