using Application.Reviews.Create;
using Application.Reviews.Update;
using Domain.Reviews;

namespace Application.Reviews;

public static class ReviewMapExtensions
{
    public static ReviewResponse ToReviewResponse(this Review review) =>
        new()
        {
            Id = review.Id,
            Title = review.Title,
            Content = review.Content,
            ImageUrl = review.ImageUrl,
            Status = review.Status.ToString(),
            FirstName = review.User.FirstName,
            LastName = review.User.LastName,
            AirlineName = review.Airline.Name
        };

    public static Review ToReview(this CreateReviewCommand command, Guid userId) =>
        Review.Create(userId, command.AirlineId, command.Title, command.Content, command.ImageUrl);

    public static void UpdateReview(this Review review, UpdateReviewCommand command)
    {
        review.Title = command.Title;
        review.Content = command.Content;
        review.ImageUrl = command.ImageUrl;
    }
}
