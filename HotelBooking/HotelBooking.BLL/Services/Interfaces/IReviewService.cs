using HotelBooking.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.BLL.Services.Interfaces;

public interface IReviewService
{
    Task<Review> CreateReviewAsync(int bookingId, int rating, string comment);
    Task<IEnumerable<Review>> GetRoomReviewsAsync(int roomId);
    Task<bool> CanUserReviewAsync(int bookingId, int userId);
}
