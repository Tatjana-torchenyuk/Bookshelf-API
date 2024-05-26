using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels
{
    public class PublisherCreateViewModel
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

    }
}
