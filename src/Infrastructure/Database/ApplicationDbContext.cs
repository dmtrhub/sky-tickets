using Application.Abstractions.Data;
using Domain.Airlines;
using Domain.Flights;
using Domain.Reservations;
using Domain.Reviews;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IMediator mediator)
    : DbContext(options), IApplicationDbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<Entity>()
        .Where(x => x.Entity.DomainEvents.Count != 0)
        .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
