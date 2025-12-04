using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.DAL.Models;

using HotelBooking.BLL.ViewModels.Rooms;

namespace HotelBooking.BLL.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomAsync(int roomId);
    Task CreateRoomAsync(RoomViewModel model);
    Task UpdateRoomAsync(RoomViewModel model);
    Task DeleteRoomAsync(int id);
}
