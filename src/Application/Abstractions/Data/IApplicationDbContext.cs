using Domain.Airlines;
using Domain.Flights;
using Domain.Reservations;
using Domain.Reviews;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Flight> Flights { get; }
    DbSet<Airline> Airlines { get; }
    DbSet<Reservation> Reservations { get; }
    DbSet<Review> Reviews { get; }
}
