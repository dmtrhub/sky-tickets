﻿namespace Domain;

public sealed class User
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; } 
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; } // M or F
    public UserRole Role { get; set; } = UserRole.Passenger; // Passenger or Administrator
    public List<Reservation> Reservations { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}
