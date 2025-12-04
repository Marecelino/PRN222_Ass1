
using HotelBooking.DAL.Models;

namespace HotelBooking.DAL.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
    Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
    Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
    Task<IEnumerable<Booking>> GetBookingsInRangeAsync(DateTime fromDate, DateTime toDate);
}
