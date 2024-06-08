using System.ComponentModel.DataAnnotations;

namespace Lib.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public ICollection<Author> Authors { get; set; } = new List<Author>();
        public Publisher? Publisher { get; set; }

    }
}
