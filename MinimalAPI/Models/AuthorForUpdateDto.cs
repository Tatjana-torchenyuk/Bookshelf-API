using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class AuthorForUpdateDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
