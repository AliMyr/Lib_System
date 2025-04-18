using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<Reservation> GetAllReservations();
        int CreateReservation(Reservation res);
        bool UpdateReservation(Reservation res);
        bool DeleteReservation(int id);
    }
}
