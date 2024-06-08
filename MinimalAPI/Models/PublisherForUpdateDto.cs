using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class PublisherForUpdateDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
