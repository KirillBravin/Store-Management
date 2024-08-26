Welcome to the store management app.

Using this app you can create, modify, delete and check all products, users and orders in your store.

To initiate the app, input these commands in API terminal:

"dotnet ef migrations add InitialCreate"

"dotnet ef database update"

Additional information:
This app was mostly written using: C#, Entity Framework, ASP.NET API, MongoDB Driver, Moq and Serilog.
I used OOP principles. Data was mostly handles by both SQL and NoSQL. This app also logs data using Serilog.
If you're interested in visual, there are screenshots of my graphs that I made. Sadly I lost the initial graph due to accident, but the pictures should still provide the general idea.
I also use separation of logic, by using repositories for my databases data, services for business logic and controllers for my RESTful APIes.
