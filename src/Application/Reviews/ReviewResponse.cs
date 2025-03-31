namespace Application.Reviews;

public sealed record ReviewResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Content { get; init; }
    public string? ImageUrl { get; init; }
    public string Status { get; init; }

    // User
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Airline
    public string AirlineName { get; set; }
}
