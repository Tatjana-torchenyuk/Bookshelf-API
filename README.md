# Books API
This project is a comprehensive solution built with C# and ASP.NET Core, designed to manage Books, Publishers, and Authors. It consists of three main components: an MVC project, a minimal API project, and a shared library with services, entities, and the DbContext.

## Features
### Books Management: 
Create, read, update, and delete books. Each book can have multiple authors and is associated with a single publisher.
### Authors Management: 
Create, read, update, and delete authors. Authors can be assigned to multiple books.
Assign Author to Book: Special endpoint to assign author to a book.
### Publishers Management: 
Create, read, update, and delete publishers. Each publisher can publish multiple books.
Assign Publisher to Book: Special endpoint to assign publisher to a book.

## Components
### MVC Project
Provides a user interface for managing books, authors, and publishers.
Includes views and controllers for handling HTTP requests.
### Minimal API Project
Offers a lightweight API for managing books, authors, and publishers.
Utilizes ASP.NET Core minimal APIs.
### Shared Library
Contains shared services, entities, and the DbContext.
Provides a central place for defining the data models, business logic, and database access.
