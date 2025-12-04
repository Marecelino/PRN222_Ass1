using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.DAL.Models;
using HotelBooking.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.BLL.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookingRepository _bookingRepository;

    public ReviewService(IReviewRepository reviewRepository, IBookingRepository bookingRepository)
    {
        _reviewRepository = reviewRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Review> CreateReviewAsync(int bookingId, int rating, string comment)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        if (booking.Status != BookingStatus.CheckedOut)
        {
            throw new InvalidOperationException("Only checked-out bookings can be reviewed.");
        }

        var existingReview = await _reviewRepository.GetReviewByBookingIdAsync(bookingId);
        if (existingReview != null)
        {
            throw new InvalidOperationException("This booking has already been reviewed.");
        }

        var review = new Review
        {
            BookingId = bookingId,
            Rating = rating,
            Comment = comment,
            CreatedDate = DateTime.Now
        };

        await _reviewRepository.AddAsync(review);
        return review;
    }

    public async Task<IEnumerable<Review>> GetRoomReviewsAsync(int roomId)
    {
        return await _reviewRepository.GetReviewsByRoomIdAsync(roomId);
    }

    public async Task<bool> CanUserReviewAsync(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.UserId != userId)
        {
            return false;
        }

        if (booking.Status != BookingStatus.CheckedOut)
        {
            return false;
        }

        var existingReview = await _reviewRepository.GetReviewByBookingIdAsync(bookingId);
        return existingReview == null;
    }
}
