using System.ComponentModel.DataAnnotations;

namespace BooksMVC.ViewModels
{
    public class PublisherCreateViewModel
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

    }
}
