using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels.RoomTypes;
using HotelBooking.DAL.Models;
using HotelBooking.DAL.Repositories;

namespace HotelBooking.BLL.Services.Implementations;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
    {
        return await _roomTypeRepository.GetAllAsync();
    }

    public async Task<RoomType?> GetRoomTypeByIdAsync(int id)
    {
        return await _roomTypeRepository.GetByIdAsync(id);
    }

    public async Task CreateRoomTypeAsync(RoomTypeViewModel model)
    {
        var roomType = new RoomType
        {
            TypeName = model.TypeName,
            Description = model.Description,
            MaxOccupancy = model.MaxOccupancy,
            PricePerNight = model.PricePerNight
        };

        await _roomTypeRepository.AddAsync(roomType);
    }

    public async Task UpdateRoomTypeAsync(RoomTypeViewModel model)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(model.RoomTypeId);
        if (roomType != null)
        {
            roomType.TypeName = model.TypeName;
            roomType.Description = model.Description;
            roomType.MaxOccupancy = model.MaxOccupancy;
            roomType.PricePerNight = model.PricePerNight;

            await _roomTypeRepository.UpdateAsync(roomType);
        }
    }

    public async Task DeleteRoomTypeAsync(int id)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(id);
        if (roomType != null)
        {
            await _roomTypeRepository.DeleteAsync(roomType);
        }
    }
}
