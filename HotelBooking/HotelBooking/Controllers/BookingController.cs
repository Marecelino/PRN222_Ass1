using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;

    public BookingController(IBookingService bookingService, IRoomService roomService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut, int guests)
    {
        var room = await _roomService.GetRoomAsync(roomId);
        if (room == null)
            return NotFound();

        var nights = (checkOut - checkIn).Days;
        var model = new CreateBookingViewModel
        {
            RoomId = roomId,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            NumberOfGuests = guests,
            RoomNumber = room.RoomNumber,
            TypeName = room.RoomType.TypeName,
            TotalPrice = room.RoomType.PricePerNight * nights
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var booking = await _bookingService.CreateBookingAsync(model, userId);
            TempData["Success"] = "Booking created successfully!";
            return RedirectToAction("Confirmation", new { id = booking.BookingId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(int id)
    {
        var booking = await _bookingService.GetBookingDetailsAsync(id);
        if (booking == null)
            return NotFound();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (booking.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Staff"))
            return Forbid();

        return View(booking);
    }

    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return View(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _bookingService.GetBookingDetailsAsync(id);
        if (booking == null)
            return NotFound();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (booking.UserId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var success = await _bookingService.CancelBookingAsync(id);
        if (success)
            TempData["Success"] = "Booking cancelled successfully";
        else
            TempData["Error"] = "Unable to cancel booking";

        return RedirectToAction("MyBookings");
    }
}
