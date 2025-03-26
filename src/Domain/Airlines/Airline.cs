using Domain.Flights;
using Domain.Reviews;
using SharedKernel;

namespace Domain.Airlines;

public sealed class Airline : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactInfo { get; set; }
    public List<Flight> Flights { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}
