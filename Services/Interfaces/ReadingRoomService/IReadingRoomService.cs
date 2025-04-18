using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReadingRoomService
    {
        IEnumerable<ReadingRoom> GetAllRooms();
        int CreateRoom(ReadingRoom room);
        bool UpdateRoom(ReadingRoom room);
        bool DeleteRoom(int id);
    }
}
