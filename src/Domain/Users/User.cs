using Domain.Reservations;
using Domain.Reviews;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; } // Male or Female
    public UserRole Role { get; set; } = UserRole.Passenger; // Passenger or Administrator
    public List<Reservation> Reservations { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];

    public User(string firstName, string lastName, string email, string passwordHash, DateOnly dateOfBirth, Gender gender)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public static User Create(string firstName, string lastName, string email, string passwordHash, DateOnly dateOfBirth, Gender gender) =>
        new(firstName, lastName, email, passwordHash, dateOfBirth, gender);
}
