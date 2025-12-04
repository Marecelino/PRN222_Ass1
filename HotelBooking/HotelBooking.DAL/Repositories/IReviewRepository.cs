using HotelBooking.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.DAL.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetReviewsByRoomIdAsync(int roomId);
    Task<Review?> GetReviewByBookingIdAsync(int bookingId);
}
