using HotelBooking.DAL.Data;
using HotelBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DAL.Repositories;

public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
{
    public RoomTypeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RoomType?> GetRoomTypeWithRoomsAsync(int roomTypeId)
    {
        return await _context.RoomTypes
            .Include(rt => rt.Rooms)
            .FirstOrDefaultAsync(rt => rt.RoomTypeId == roomTypeId);
    }
}
