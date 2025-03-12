# Sky Tickets ✈🎫

✈ Overview:
The Online Flight Reservation System allows users to search, book, and review flights, with distinct functionalities based on user roles (Unregistered, Registered/Passenger, Admin). The system is powered by ASP.NET Web API 2, Clean Architecture, and uses EF Core for data access.

✈ Technologies:
Backend: C#, .NET (ASP.NET Web API 2), EF Core, Minimal API
Authentication: JWT
Frontend: jQuery, AJAX
Database: SQL Server
Architecture: Clean Architecture, CQRS, RBAC

✈ Features:
Unregistered Users: View flights, search, sort, and filter.
Registered Users: Book flights, view past bookings, and leave reviews.
Admin: Manage users, airlines, flights, and reservations. Approve or reject reviews.
JWT Authentication and RBAC for access control.

✈ Setup:
Clone the repository:
git clone https://github.com/username/flight-reservation-system.git

Install dependencies using NuGet or the .NET CLI.

Set up SQL Server and configure the database connection.

Run the application using Visual Studio or through the .NET CLI.

✈ Usage:
Navigate to the home page to view available flights.
Use search filters to find flights by destination, date, and airline.
Registered users can log in and manage their reservations.
Admins have access to user management and flight/airline CRUD operations.

✈ Future Improvements:
Expand testing with xUnit.
Add caching and logging using Redis and Serilog.
This README will be updated with more details as the project progresses.
