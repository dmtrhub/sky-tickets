using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public enum FlightStatus
{
    Active,
    Canceled,
    Completed
}

public enum ReservationStatus
{
    Created,
    Approved,
    Canceled,
    Completed
}

public enum ReviewStatus
{
    Created,
    Approved,
    Rejected
}
