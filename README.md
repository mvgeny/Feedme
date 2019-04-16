# Feedme
This project is an architecture template for ASP.NET Core applications with some good practices. It adheres to the object-oriented design principles and aspect-oriented paradigm. It also applies a few appropriate functional techniques. Enjoy!
## Project structure
![Project structure](https://raw.githubusercontent.com/mvgeny/feedme/master/docs/structure.png)
- **Domain** layer is a library containing just POCO classes with base business logic. It is pure of any dependencies.
- **Data** layer takes responsibility for isolation of data persistence, migrations, Entity configurations, implementing Repositories.
- **Infrastructure** layer contains classes for accessing external resources. In our case, it is reading and parsing RSS feeds from the web.
- **Application** layer is a place to keep our commands, queries, dispatching them and everything with regard to this.
## Practices
- SOLID
- DDD
- CQRS (single db implementation without Mediatr)
- Inversion of Control
- Thin controllers
- Decorator wrapping (caching and logging)
- Functional style exception handling
- Transactional integration testing setup
## Technologies
- ASP.NET Core 2.2
- EF Core 2.2
- MSSQL
- Dapper
- Serilog
- xUnit
- FluentAssertions
- Autofac
- Swashbuckle
## Implemented task
Create an back-end web service for providing news streams from different
sources. Service should function like [Feedly service](https://feedly.com/).

Service should allow clients to manage feed collections, and to access all news items in those collections.
Service should extensively cache data to minimize external requests. As a start, use RSS or Atom feeds,
but architecture should be extensible to support adding new types of feed sources in the future (3rd
party services, web scraping, etc).

Service should return data in its own format, you should create custom models for all data types, not
just redirect XML feeds to the clients.
This should be a headless Web API REST service, no UI is necessary.

Necessary functions:
- Create a new collection (returns collection Id)
- Add feed to a collection
- Get all news for a collection
- Caching of feed data
- Persistence
- Logging
## Resources

<details>
<summary>Links</summary>
- [Clean Architecture with ASP.NET Core 2.2 speech by Jason Taylor](https://www.youtube.com/watch?v=Zygw4UAxCdg)
- [Applying Functional Principles in C# course by Vladimir Khorikov](https://app.pluralsight.com/library/courses/csharp-applying-functional-principles)
- [Encapsulation and SOLID course by Mark Seemann](https://app.pluralsight.com/library/courses/encapsulation-solid)
- [Ordering service at eShopOnContainers by Microsoft](https://github.com/dotnet-architecture/eShopOnContainers/tree/dev/src/Services/Ordering)
</details>

<details>
<summary>Books</summary>
- [Dependency Injection Principles, Practices, and Patterns](https://www.manning.com/books/dependency-injection-principles-practices-patterns)
- [Clean Architecture: A Craftsman's Guide to Software Structure and Design](https://www.amazon.com/gp/product/B075LRM681/)
- [Domain-Driven Design: Tackling Complexity in the Heart of Software](https://www.amazon.com/gp/product/B00794TAUG/)
</details>
## Setup
1. [NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2) and [MSSQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) are required
2. Clone the repo
3. Go to `\src\Feedme.Api` directory and execute ```dotnet run``` command
4. The app is running on [http://localhost:5000/](http://localhost:5000/) endpoint
5. Simply try it via tool like Postman or built-in `/swagger` UI interface