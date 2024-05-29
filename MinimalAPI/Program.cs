//using AutoMapper;
using AutoMapper;
using Lib.DbContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register the DbContext on the container, getting the connection string from appSettings
var connection = builder.Configuration["ConnectionStrings:BooksDBConnectionString"];
builder.Services.AddDbContext<BooksDbContext>(o => o.UseMySql(connection, ServerVersion.AutoDetect(connection), mySqlOptions => mySqlOptions.MigrationsAssembly("MinimalAPI")));

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Exception handling
builder.Services.AddProblemDetails();

var app = builder.Build();

//-------------------------------------
// Configure the HTTP request pipeline.
//-------------------------------------

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
} else {
    app.UseExceptionHandler();
}


//--Authors Endpoints--

var authorsEndpoints = app.MapGroup("/api/authors");
var authorWithIdEndpoints = authorsEndpoints.MapGroup("/{authorid}");
var authorBooksEndpoints = authorWithIdEndpoints.MapGroup("/books");
var assignBookToAuthorEndpoints = authorsEndpoints.MapGroup("/assign-book");

authorsEndpoints.MapGet("", Ok<IEnumerable<AuthorDto>> (
    BooksDbContext booksDbContext,
    IMapper mapper,
    ILogger<AuthorDto> logger)
    => {
        logger.LogInformation("Getting the authors");
        return TypedResults.Ok(mapper.Map<IEnumerable<AuthorDto>>(
            booksDbContext.Authors));
    });

authorWithIdEndpoints.MapGet("", Results<NotFound, Ok<AuthorDto>> (
    BooksDbContext booksDbContext,
    IMapper mapper,
    ILogger<AuthorDto> logger,
    int authorId)
    => {
        logger.LogInformation("Getting author");
        var authorEntity = booksDbContext.Authors.FirstOrDefault(x => x.Id == authorId);
        
        if (authorEntity is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<AuthorDto>(authorEntity));
    }).WithName("GetAuthor");

authorBooksEndpoints.MapGet("", Results<NotFound, Ok<AuthorBooksDto>> (
    BooksDbContext bookDbContext,
    IMapper mapper,
    int authorId) 
    => {
        var books = bookDbContext.Books.Where(x => x.Id == authorId);
        
        if (books is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<AuthorBooksDto>(books));
    });

//--Books Endpoints--

var booksEndpoints = app.MapGroup("/api/books");
var bookWithIdEndpoints = booksEndpoints.MapGroup("/{bookid}");
var bookAuthorsEndpoints = bookWithIdEndpoints.MapGroup("/authors");

//--Publishers Endpoints--
var publishersEndpoints = app.MapGroup("/api/publishers");
var publisherWithIdEndpoints = publishersEndpoints.MapGroup("/{publisherid}");
var publisherBooksEndpoints = publisherWithIdEndpoints.MapGroup("/books");
var assignBookToPublisherEndpoints = publishersEndpoints.MapGroup("/assign-book");

//-------------------------------------------
// Recontruct dB when starting up application
//-------------------------------------------

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope()) {
    var context = serviceScope.ServiceProvider.GetRequiredService<BooksDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();


