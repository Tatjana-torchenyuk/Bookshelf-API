using AutoMapper;
using Lib.DbContexts;
using Lib.Entities;
using Lib.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// BooksRepository
builder.Services.AddScoped<IBooksRepository, EfBooksRepository>();

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

// GET

authorsEndpoints.MapGet("", Ok<IEnumerable<AuthorDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper)
    => {
        var authors = mapper.Map<IEnumerable<AuthorDto>>(booksRepository.GetAllAuthors());
        return TypedResults.Ok(authors);
    });

authorWithIdEndpoints.MapGet("", Results<NotFound, Ok<AuthorDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromRoute] int authorid)
    => {
        var authorEntity = booksRepository.GetAuthorById(authorid);

        if (authorEntity is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<AuthorDto>(authorEntity));
    }).WithName("GetAuthor");

authorBooksEndpoints.MapGet("", Results<NotFound, Ok<IEnumerable<AuthorBooksDto>>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromRoute] int authorid)
    => {
        var books = booksRepository.GetBooksByAuthorId(authorid);

        if (books is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<IEnumerable<AuthorBooksDto>>(books));
    });

// POST

authorsEndpoints.MapPost("", CreatedAtRoute<AuthorDto> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] AuthorForCreationDto authorForCreationDto)
    => {
        var authorEntity = mapper.Map<Author>(authorForCreationDto);
        booksRepository.AddAuthor(authorEntity);

        var authorToReturn = mapper.Map<AuthorDto>(authorEntity);

        return TypedResults.CreatedAtRoute(
            authorToReturn,
            "GetAuthor",
            new { authorid = authorToReturn.Id });
    });

// PUT

authorWithIdEndpoints.MapPut("", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] AuthorForUpdateDto authorForUpdateDto,
    [FromRoute] int authorid)
    => {
        var authorEntity = booksRepository.GetAuthorById(authorid);
        if (authorEntity is null) {
            return TypedResults.NotFound();
        }

        authorEntity.Name = authorForUpdateDto.Name;
        booksRepository.UpdateAuthor(authorEntity);

        return TypedResults.NoContent();
    });

app.MapPut("/api/authors/assign-to-book", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromBody] AuthorToBookUpdateDto authorToBookUpdateDto)
    => {
        var authorEntity = booksRepository.GetAuthorById(authorToBookUpdateDto.AuthorId);
        var bookEntity = booksRepository.GetBookById(authorToBookUpdateDto.BookId);

        if (bookEntity is null || authorEntity is null) {
            return TypedResults.NotFound();
        }

        booksRepository.UpdateAuthorToBook(bookEntity, authorEntity);

        return TypedResults.NoContent();

    });

// DELETE

authorWithIdEndpoints.MapDelete("", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromRoute] int authorid)
    => {
        var authorEntity = booksRepository.GetAuthorById(authorid);
        if (authorEntity is null) {
            return TypedResults.NotFound();
        }
        booksRepository.DeleteAuthor(authorEntity);
        return TypedResults.NoContent();
    });

//--Books Endpoints--

var booksEndpoints = app.MapGroup("/api/books");
var bookWithIdEndpoints = booksEndpoints.MapGroup("/{bookid}");
var bookAuthorsEndpoints = bookWithIdEndpoints.MapGroup("/authors");

// GET

booksEndpoints.MapGet("", Ok<IEnumerable<BookDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper)
    => {
        var books = mapper.Map<IEnumerable<BookDto>>(booksRepository.GetAllBooks());
        return TypedResults.Ok(books);
    });

bookWithIdEndpoints.MapGet("", Results<NotFound, Ok<BookDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromRoute] int bookid)
    => {
        var bookEntity = booksRepository.GetBookById(bookid);

        if (bookEntity is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<BookDto>(bookEntity));
    }).WithName("GetBook");

bookAuthorsEndpoints.MapGet("", Results<NotFound, Ok<IEnumerable<AuthorDto>>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromRoute] int bookid)
    => {
        var authors = booksRepository.GetAuthorsByBookId(bookid);

        if (authors is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<IEnumerable<AuthorDto>>(authors));
    });

// POST

booksEndpoints.MapPost("", Results<NotFound, CreatedAtRoute<BookDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] BookForCreationDto bookForCreationDto)
    => {
        var publisherId = bookForCreationDto.PublisherId;
        var publisherEntity = booksRepository.GetPublisherById(publisherId);
        if (publisherEntity is null) {
            return TypedResults.NotFound();
        }

        var bookEntity = mapper.Map<Book>(bookForCreationDto);
        bookEntity.Publisher = publisherEntity;
        
        booksRepository.AddBook(bookEntity);

        var bookToReturn = mapper.Map<BookDto>(bookEntity);

        return TypedResults.CreatedAtRoute(
            bookToReturn,
            "GetBook",
            new { bookid = bookToReturn.Id });
    });

// PUT

bookWithIdEndpoints.MapPut("", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] BookForUpdateDto bookForUpdateDto,
    [FromRoute] int bookid)
    => {
        var bookEntity = booksRepository.GetBookById(bookid);
        if (bookEntity is null) {
            return TypedResults.NotFound();
        }

        bookEntity.Title = bookForUpdateDto.Title;
        bookEntity.ISBN = bookForUpdateDto.ISBN;

        booksRepository.UpdateBook(bookEntity);

        return TypedResults.NoContent();
    });


// DELETE

bookWithIdEndpoints.MapDelete("", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromRoute] int bookid)
    => {
        var bookEntity = booksRepository.GetBookById(bookid);
        if (bookEntity is null) {
            return TypedResults.NotFound();
        }
        booksRepository.DeleteBook(bookEntity);
        return TypedResults.NoContent();
    });

//--Publishers Endpoints--
var publishersEndpoints = app.MapGroup("/api/publishers");
var publisherWithIdEndpoints = publishersEndpoints.MapGroup("/{publisherid}");
var publisherBooksEndpoints = publisherWithIdEndpoints.MapGroup("/books");

// GET

publishersEndpoints.MapGet("", Ok<IEnumerable<PublisherDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper)
    => {
        var publishers = mapper.Map<IEnumerable<PublisherDto>>(booksRepository.GetAllPublishers());
        return TypedResults.Ok(publishers);
    });

publisherWithIdEndpoints.MapGet("", Results<NotFound, Ok<PublisherDto>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromRoute] int publisherid)
    => {
        var publisherEntity = booksRepository.GetPublisherById(publisherid);

        if (publisherEntity is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<PublisherDto>(publisherEntity));
    }).WithName("GetPublisher");

publisherBooksEndpoints.MapGet("", Results<NotFound, Ok<IEnumerable<PublisherBooksDto>>> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromRoute] int publisherid)
    => {
        var books = booksRepository.GetBooksByPublisherId(publisherid);

        if (books is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<IEnumerable<PublisherBooksDto>>(books));
    });

// POST

publishersEndpoints.MapPost("", CreatedAtRoute<PublisherDto> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] PublisherForCreationDto publisherForCreationDto)
    => {
        var publisherEntity = mapper.Map<Publisher>(publisherForCreationDto);
        booksRepository.AddPublisher(publisherEntity);

        var publisherToReturn = mapper.Map<PublisherDto>(publisherEntity);

        return TypedResults.CreatedAtRoute(
            publisherToReturn,
            "GetPublisher",
            new { publisherid = publisherToReturn.Id });
    });

// PUT

publisherWithIdEndpoints.MapPut("", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] PublisherForUpdateDto publisherForUpdateDto,
    [FromRoute] int publisherid)
    => {
        var publisherEntity = booksRepository.GetPublisherById(publisherid);
        if (publisherEntity is null) {
            return TypedResults.NotFound();
        }

        publisherEntity.Name = publisherForUpdateDto.Name;
        booksRepository.UpdatePublisher(publisherEntity);

        return TypedResults.NoContent();
    });

app.MapPut("/api/publishers/assign-to-book", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromServices] IMapper mapper,
    [FromBody] PublisherToBookUpdateDto publisherToBookUpdateDto)
    => {
        var publisherEntity = booksRepository.GetPublisherById(publisherToBookUpdateDto.PublisherId);
        var bookEntity = booksRepository.GetBookById(publisherToBookUpdateDto.BookId);

        if (bookEntity is null || publisherEntity is null) {
            return TypedResults.NotFound();
        }

        booksRepository.UpdatePublisherToBook(bookEntity, publisherEntity);

        return TypedResults.NoContent();

    });

// DELETE

publisherWithIdEndpoints.MapDelete("", Results<NotFound, NoContent> (
    [FromServices] IBooksRepository booksRepository,
    [FromRoute] int publisherid)
    => {
        var publisherEntity = booksRepository.GetPublisherById(publisherid);
        if (publisherEntity is null) {
            return TypedResults.NotFound();
        }
        booksRepository.DeletePublisher(publisherEntity);
        return TypedResults.NoContent();
    });

//--------------------------------------------------------------
// Recreate & migrate the database on each run, for demo purpose
//--------------------------------------------------------------

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope()) {
    var context = serviceScope.ServiceProvider.GetRequiredService<BooksDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();


