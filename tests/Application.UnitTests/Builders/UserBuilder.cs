using Domain;
using Domain.Reservations;
using Domain.Reviews;
using Domain.Users;

namespace Application.UnitTests.Builders;

public class UserBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _firstName = "Test";
    private string _lastName = "User";
    private string _email = "test@example.com";
    private string _passwordHash = "hashedpassword";
    private DateOnly _dateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20));
    private UserRole _role = UserRole.Passenger;
    private Gender _gender = Gender.Male;
    private readonly List<Reservation> _reservations = [];
    private readonly List<Review> _reviews = [];

    public UserBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithRole(UserRole role)
    {
        _role = role;
        return this;
    }

    public UserBuilder WithName(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public UserBuilder WithDateOfBirth(DateOnly dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }

    public UserBuilder WithReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
        return this;
    }

    public UserBuilder WithReview(Review review)
    {
        _reviews.Add(review);
        return this;
    }

    public User Build()
    {
        var user = User.Create(
            _firstName, 
            _lastName, 
            _email, 
            _passwordHash, 
            _dateOfBirth,
            _gender);

        typeof(User)
            .GetProperty(nameof(User.Id))!
            .SetValue(user, _id);

        typeof(User)
            .GetProperty(nameof(User.Role))!
            .SetValue(user, _role);

        typeof(User)
            .GetProperty(nameof(User.Reservations))!
            .SetValue(user, _reservations);

        typeof(User)
            .GetProperty(nameof(User.Reviews))!
            .SetValue(user, _reviews);

        return user;
    }
}
