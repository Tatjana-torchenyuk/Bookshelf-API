﻿using System.ComponentModel.DataAnnotations;

namespace Lib.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
