﻿using Lib.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lib.DbContexts
{
    public class BooksDbContext : DbContext
    {
        public DbSet<Book> Books { get; set;}
        public DbSet<Author> Authors { get; set;}
        public DbSet<Publisher> Publishers { get; set;}
        public BooksDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}