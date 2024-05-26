using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels
{
    public class AuthorUpdateViewModel
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
