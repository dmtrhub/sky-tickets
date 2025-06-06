﻿using Domain.Airlines;
using Domain.Users;
using SharedKernel;

namespace Domain.Reviews;

public sealed class Review : Entity
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

    public Review(Guid userId, Guid airlineId, string title, string content, string? imageUrl)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AirlineId = airlineId;
        Title = title;
        Content = content;
        ImageUrl = imageUrl;
    }

    public static Review Create(Guid userId, Guid airlineId, string title, string content, string? imageUrl) =>
        new(userId, airlineId, title, content, imageUrl);
}
