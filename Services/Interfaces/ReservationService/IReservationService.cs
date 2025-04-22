using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<ReservationViewModel> GetAllReservationDetails();
        Reservation GetReservationById(int id);
        int CreateReservation(Reservation res);
        bool UpdateReservation(Reservation res);
        bool DeleteReservation(int id);
    }
}