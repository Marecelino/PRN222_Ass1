using HotelBooking.DAL.Models;

namespace HotelBooking.DAL.Repositories;

public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int occupancy);
    Task<Room?> GetRoomWithTypeAsync(int roomId);
}
