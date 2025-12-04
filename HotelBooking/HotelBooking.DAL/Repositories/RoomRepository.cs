using HotelBooking.DAL.Data;

using HotelBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DAL.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int occupancy)
    {
        // Get all rooms with sufficient capacity
        var allRooms = await _context.Rooms
            .Include(r => r.RoomType)
            .Where(r => r.Status == RoomStatus.Available && r.RoomType.MaxOccupancy >= occupancy)
            .ToListAsync();

        // Get booked room IDs for the date range
        var bookedRoomIds = await _context.Bookings
            .Where(b => b.Status != BookingStatus.Cancelled &&
                       b.CheckInDate < checkOut &&
                       b.CheckOutDate > checkIn)
            .Select(b => b.RoomId)
            .ToListAsync();

        // Return rooms that are not booked
        return allRooms.Where(r => !bookedRoomIds.Contains(r.RoomId));
    }

    public async Task<Room?> GetRoomWithTypeAsync(int roomId)
    {
        return await _context.Rooms
            .Include(r => r.RoomType)
            .FirstOrDefaultAsync(r => r.RoomId == roomId);
    }
}
