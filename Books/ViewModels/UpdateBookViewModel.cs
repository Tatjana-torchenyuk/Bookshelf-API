using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels
{
    public class UpdateBookViewModel
    {
        [Required, StringLength(150)]
        public string Title { get; set; }
        [Required, StringLength(15)]
        public string ISBN { get; set; }
    }
}
