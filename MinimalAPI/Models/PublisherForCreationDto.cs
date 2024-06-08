using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class PublisherForCreationDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
