using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class AuthorForCreationDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
