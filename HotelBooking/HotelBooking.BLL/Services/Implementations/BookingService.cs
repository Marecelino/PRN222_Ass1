using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HotelBooking.DAL.Models;
using HotelBooking.DAL.Repositories;
using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels.Bookings;
using HotelBooking.BLL.ViewModels;


namespace HotelBooking.BLL.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomSearchResultViewModel>> SearchAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int numberOfGuests)
    {
        if (checkOut <= checkIn)
            throw new ArgumentException("Check-out date must be after check-in date");

        var availableRooms = await _roomRepository.GetAvailableRoomsAsync(checkIn, checkOut, numberOfGuests);
        var nights = (checkOut - checkIn).Days;

        return availableRooms.Select(room => new RoomSearchResultViewModel
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            TypeName = room.RoomType.TypeName,
            Description = room.RoomType.Description,
            MaxOccupancy = room.RoomType.MaxOccupancy,
            PricePerNight = room.RoomType.PricePerNight,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            NumberOfNights = nights,
            TotalPrice = room.RoomType.PricePerNight * nights
        }).ToList();
    }

    public async Task<Booking> CreateBookingAsync(CreateBookingViewModel model, int userId)
    {
        var availableRooms = await _roomRepository.GetAvailableRoomsAsync(
            model.CheckInDate, model.CheckOutDate, model.NumberOfGuests);

        if (!availableRooms.Any(r => r.RoomId == model.RoomId))
            throw new InvalidOperationException("Your booking exceeds the room’s maximum occupancy(Number Of Guests) ");

        var room = await _roomRepository.GetRoomWithTypeAsync(model.RoomId);
        if (room == null)
            throw new ArgumentException("Room not found");

        var nights = (model.CheckOutDate - model.CheckInDate).Days;
        var totalPrice = room.RoomType.PricePerNight * nights;

        var booking = new Booking
        {
            UserId = userId,
            RoomId = model.RoomId,
            CheckInDate = model.CheckInDate,
            CheckOutDate = model.CheckOutDate,
            NumberOfGuests = model.NumberOfGuests,
            Status = BookingStatus.Confirmed,
            TotalPrice = totalPrice,
            BookingDate = DateTime.Now,
            SpecialRequests = model.SpecialRequests
        };

        return await _bookingRepository.AddAsync(booking);
    }

    public async Task<bool> CheckInAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.Status != BookingStatus.Confirmed)
            return false;

        booking.Status = BookingStatus.CheckedIn;
        booking.ActualCheckInTime = DateTime.Now;
        await _bookingRepository.UpdateAsync(booking);
        return true;
    }

    public async Task<bool> CheckOutAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.Status != BookingStatus.CheckedIn)
            return false;

        booking.Status = BookingStatus.CheckedOut;
        booking.ActualCheckOutTime = DateTime.Now;
        await _bookingRepository.UpdateAsync(booking);
        return true;
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.Status == BookingStatus.CheckedOut || booking.Status == BookingStatus.Cancelled)
            return false;

        booking.Status = BookingStatus.Cancelled;
        await _bookingRepository.UpdateAsync(booking);
        return true;
    }

    public async Task<DashboardViewModel> GetDashboardStatsAsync(DateTime fromDate, DateTime toDate)
    {
        var bookings = await _bookingRepository.GetBookingsInRangeAsync(fromDate, toDate);
        var allRooms = await _roomRepository.GetAllAsync();

        var totalDays = (toDate - fromDate).Days;
        var totalRoomNights = allRooms.Count() * totalDays;

        var occupiedNights = bookings
            .Where(b => b.Status == BookingStatus.CheckedIn || b.Status == BookingStatus.CheckedOut)
            .Sum(b => (b.CheckOutDate - b.CheckInDate).Days);

        var occupancyRate = totalRoomNights > 0 ? (decimal)occupiedNights / totalRoomNights * 100 : 0;

        var totalRevenue = bookings
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Sum(b => b.TotalPrice);

        var bookingsByType = bookings
            .Where(b => b.Status != BookingStatus.Cancelled)
            .GroupBy(b => b.Room.RoomType.TypeName)
            .Select(g => new BookingsByRoomType
            {
                RoomTypeName = g.Key,
                BookingCount = g.Count()
            })
            .ToList();

        var dailyRevenues = bookings
            .Where(b => b.Status != BookingStatus.Cancelled)
            .GroupBy(b => b.CheckInDate.Date)
            .Select(g => new DailyRevenue
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                Revenue = g.Sum(b => b.TotalPrice)
            })
            .OrderBy(d => d.Date)
            .ToList();

        return new DashboardViewModel
        {
            FromDate = fromDate,
            ToDate = toDate,
            OccupancyRate = Math.Round(occupancyRate, 2),
            TotalRevenue = totalRevenue,
            TotalBookings = bookings.Count(),
            BookingsByRoomType = bookingsByType,
            DailyRevenues = dailyRevenues
        };
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
    {
        return await _bookingRepository.GetUserBookingsAsync(userId);
    }

    public async Task<Booking?> GetBookingDetailsAsync(int bookingId)
    {
        return await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
    }
}
