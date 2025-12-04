using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.DAL.Models;
using HotelBooking.BLL.ViewModels.Bookings;
using HotelBooking.BLL.ViewModels;


namespace HotelBooking.BLL.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<RoomSearchResultViewModel>> SearchAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int numberOfGuests);
    Task<Booking> CreateBookingAsync(CreateBookingViewModel model, int userId);
    Task<bool> CheckInAsync(int bookingId);
    Task<bool> CheckOutAsync(int bookingId);
    Task<bool> CancelBookingAsync(int bookingId);
    Task<DashboardViewModel> GetDashboardStatsAsync(DateTime fromDate, DateTime toDate);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
    Task<Booking?> GetBookingDetailsAsync(int bookingId);
}
