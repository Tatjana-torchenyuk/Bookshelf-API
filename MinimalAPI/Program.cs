using AutoMapper;
using Lib.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//--------------------------------
// Add services to the container.
//--------------------------------

// TODO: Swagger

// register the DbContext on the container, getting the connection string from appSettings
var connection = builder.Configuration["ConnectionStrings:BooksDBConnectionString"];
builder.Services.AddDbContext<BooksDbContext>(o => o.UseMySql(connection, ServerVersion.AutoDetect(connection)));

// Automapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Exception handling
builder.Services.AddProblemDetails();

var app = builder.Build();

//-------------------------------------
// Configure the HTTP request pipeline.
//-------------------------------------

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler();
}

//--Authors Endpoints--

var authorsEndpoints = app.MapGroup("/api/authors");
var authorWithIdEndpoints = authorsEndpoints.MapGroup("/{authorid}");
var authorBooksEndpoints = authorWithIdEndpoints.MapGroup("/books");
var assignBookToAuthorEndpoints = authorsEndpoints.MapGroup("/assign-book");

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


