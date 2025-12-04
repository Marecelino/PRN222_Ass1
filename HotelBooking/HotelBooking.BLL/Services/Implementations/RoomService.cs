using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.DAL.Models;
using HotelBooking.DAL.Repositories;
using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels.Rooms;

namespace HotelBooking.BLL.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetRoomAsync(int roomId)
    {
        return await _roomRepository.GetRoomWithTypeAsync(roomId);
    }

    public async Task CreateRoomAsync(RoomViewModel model)
    {
        var room = new Room
        {
            RoomNumber = model.RoomNumber,
            RoomTypeId = model.RoomTypeId,
            Status = Enum.Parse<RoomStatus>(model.Status)
        };

        await _roomRepository.AddAsync(room);
    }

    public async Task UpdateRoomAsync(RoomViewModel model)
    {
        var room = await _roomRepository.GetByIdAsync(model.RoomId);
        if (room != null)
        {
            room.RoomNumber = model.RoomNumber;
            room.RoomTypeId = model.RoomTypeId;
            room.Status = Enum.Parse<RoomStatus>(model.Status);

            await _roomRepository.UpdateAsync(room);
        }
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room != null)
        {
            await _roomRepository.DeleteAsync(room);
        }
    }
}
