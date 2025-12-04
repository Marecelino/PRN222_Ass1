using System;

using HotelBooking.DAL.Models;

namespace HotelBooking.BLL.ViewModels.Bookings;

public class BookingDetailsViewModel
{
    public Booking Booking { get; set; } = null!;
    public Room Room => Booking.Room;
    public RoomType RoomType => Booking.Room.RoomType;
    public User User => Booking.User;
    public BookingStatus Status => Booking.Status;
}
