using HotelBooking.DAL.Data;
using HotelBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.DAL.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Review>> GetReviewsByRoomIdAsync(int roomId)
    {
        return await _context.Reviews
            .Include(r => r.Booking)
            .ThenInclude(b => b.User)
            .Where(r => r.Booking.RoomId == roomId)
            .OrderByDescending(r => r.CreatedDate)
            .ToListAsync();
    }

    public async Task<Review?> GetReviewByBookingIdAsync(int bookingId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.BookingId == bookingId);
    }
}
