using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public sealed class User
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; } 
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } // M or F
    public string Role { get; set; } = "Passenger"; // Passenger(default) or Administrator
    public List<Reservation> Reservations { get; set; } = [];
}
