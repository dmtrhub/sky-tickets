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

    public Airline(string name, string address, string contactInfo)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        ContactInfo = contactInfo;
    }

    public static Airline Create(string name, string address, string contactInfo) =>
        new(name, address, contactInfo);
}
