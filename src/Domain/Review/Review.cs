using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public sealed class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid AirlineId { get; set; }
    public Airline Airline { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? ImageUrl { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Created; // Created, Approved, Rejected
}
