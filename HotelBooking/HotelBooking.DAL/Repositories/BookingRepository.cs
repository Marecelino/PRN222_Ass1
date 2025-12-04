using HotelBooking.DAL.Data;
using HotelBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DAL.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.RoomType)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.RoomType)
            .Include(b => b.User)
            .Where(b => b.Status == status)
            .OrderBy(b => b.CheckInDate)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.RoomType)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<IEnumerable<Booking>> GetBookingsInRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.RoomType)
            .Where(b => b.CheckInDate >= fromDate && b.CheckOutDate <= toDate)
            .ToListAsync();
    }
}
