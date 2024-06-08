using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class BookForUpdateDto
    {
        [Required, StringLength(150)]
        public string Title { get; set; }
        [Required, StringLength(15)]
        public string ISBN { get; set; }
    }
}
