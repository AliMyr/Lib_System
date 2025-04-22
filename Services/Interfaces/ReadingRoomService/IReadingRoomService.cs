using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReadingRoomService
    {
        IEnumerable<ReadingRoomViewModel> GetAllRoomDetails();
        ReadingRoom GetRoomById(int id);
        int CreateRoom(ReadingRoom room);
        bool UpdateRoom(ReadingRoom room);
        bool DeleteRoom(int id);
    }
}