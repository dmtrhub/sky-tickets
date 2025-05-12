using Domain;
using Domain.Airlines;
using Domain.Reviews;
using Domain.Users;

namespace Application.UnitTests.Builders;

public class ReviewBuilder
{
    private Guid _id = Guid.NewGuid();
    private readonly User _user = new UserBuilder().Build();
    private readonly Airline _airline = new AirlineBuilder().Build();
    private string _title = "Default Title";
    private string _content = "Default Content";
    private string? _imageUrl = null;
    private ReviewStatus _status = ReviewStatus.Created; // Created, Approved, Rejected

    public ReviewBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ReviewBuilder WithUserId(Guid userId)
    {
        _user.Id = userId;
        return this;
    }

    public ReviewBuilder WithAirlineId(Guid airlineId)
    {
        _airline.Id = airlineId;
        return this;
    }

    public ReviewBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ReviewBuilder WithContent(string content)
    {
        _content = content;
        return this;
    }

    public ReviewBuilder WithImageUrl(string? imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }

    public ReviewBuilder WithStatus(ReviewStatus status)
    {
        _status = status;
        return this;
    }

    public Review Build()
    {
        var review = Review.Create(
            userId: _user.Id,
            airlineId: _airline.Id,
            title: _title,
            content: _content,
            imageUrl: _imageUrl);

        typeof(Review)
            .GetProperty(nameof(Review.Id))
            ?.SetValue(review, _id);

        typeof(Review)
            .GetProperty(nameof(Review.Status))
            ?.SetValue(review, _status);

        typeof(Review)
            .GetProperty(nameof(Review.User))
            ?.SetValue(review, _user);

        typeof(Review)
            .GetProperty(nameof(Review.Airline))
            ?.SetValue(review, _airline);

        return review;
    }
}
