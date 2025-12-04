using HotelBooking.DAL.Models;

namespace HotelBooking.DAL.Repositories;

public interface IRoomTypeRepository : IRepository<RoomType>
{
    Task<RoomType?> GetRoomTypeWithRoomsAsync(int roomTypeId);
}
