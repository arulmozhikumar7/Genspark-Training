Client (Frontend / API Consumer)
    ↓ sends Request DTO (e.g., CreateDto, UpdateDto)
Controller (API Endpoint)
    ↳ Accepts Request DTO
    ↓ calls
Service (Concrete Class)
    ↳ Contains business logic
    ↳ Converts DTO → Domain Model / Entity
    ↓ calls
Repository Interface (e.g., IRepository)
    ↳ Implemented by Repository class
    ↳ Handles Data Access (CRUD operations)
    ↓ interacts with
Database (Relational / NoSQL)


DI Service Lifetime :
1.AddTransient :
Creates a new instance every time the service is requested.
2.AddScoped :
Creates one instance per client request (e.g., per HTTP request).
3.AddSingleton :
Creates one single instance for the entire application lifetime.

Task in C# :
* Task represents an asynchronous operation that can run concurrently without blocking the main thread.
* It is part of the Task Parallel Library (TPL) and commonly used with the async/await keywords to write asynchronous code.
* A Task can return a result (Task<T>) or just represent a void operation (Task).

IActionResult / ActionResult :
| Feature            | `IActionResult`                                         | `ActionResult`                                                                   |
| ------------------ | ------------------------------------------------------- | -------------------------------------------------------------------------------- |
| Type               | Interface                                               | Concrete class                                                                   |
| Return flexibility | Any implementation of `IActionResult`                   | Either an instance of `ActionResult` or a specific type (with `ActionResult<T>`) |
| Use case           | When returning only action results (e.g., Ok, NotFound) | When returning data or action results together                                   |
| Supports Generics  | No                                                      | Yes (`ActionResult<T>`)                                                          |
