using Books.Entities;
using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels
{
    public class AuthorCreateViewModel
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

    }
}
