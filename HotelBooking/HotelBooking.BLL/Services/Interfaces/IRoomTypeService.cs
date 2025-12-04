using HotelBooking.BLL.ViewModels.RoomTypes;
using HotelBooking.DAL.Models;

namespace HotelBooking.BLL.Services.Interfaces;

public interface IRoomTypeService
{
    Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
    Task<RoomType?> GetRoomTypeByIdAsync(int id);
    Task CreateRoomTypeAsync(RoomTypeViewModel model);
    Task UpdateRoomTypeAsync(RoomTypeViewModel model);
    Task DeleteRoomTypeAsync(int id);
}
