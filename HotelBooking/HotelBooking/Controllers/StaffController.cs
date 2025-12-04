using HotelBooking.DAL.Data;
using HotelBooking.DAL.Models;
using HotelBooking.DAL.Repositories;
using HotelBooking.BLL.Services;
using HotelBooking.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers;

[Authorize(Roles = "Staff,Admin")]
public class StaffController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly ApplicationDbContext _context;

    public StaffController(IBookingService bookingService, ApplicationDbContext context)
    {
        _bookingService = bookingService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Get bookings with all related data properly loaded
        var bookings = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r.RoomType)
            .Where(b => b.CheckInDate >= DateTime.Today.AddDays(-1) &&
                       b.CheckInDate <= DateTime.Today.AddDays(2) &&
                       (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn))
            .OrderBy(b => b.CheckInDate)
            .ToListAsync();

        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> CheckIn(int id)
    {
        var booking = await _bookingService.GetBookingDetailsAsync(id);
        if (booking == null)
            return NotFound();

        if (booking.Status != BookingStatus.Confirmed)
        {
            TempData["Error"] = "Only confirmed bookings can be checked in";
            return RedirectToAction("Index");
        }

        return View(booking);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmCheckIn(int id)
    {
        var success = await _bookingService.CheckInAsync(id);
        if (success)
            TempData["Success"] = "Guest checked in successfully";
        else
            TempData["Error"] = "Unable to check in guest. Booking must be in Confirmed status.";

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> CheckOut(int id)
    {
        var booking = await _bookingService.GetBookingDetailsAsync(id);
        if (booking == null)
            return NotFound();

        if (booking.Status != BookingStatus.CheckedIn)
        {
            TempData["Error"] = "Only checked-in bookings can be checked out";
            return RedirectToAction("Index");
        }

        return View(booking);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmCheckOut(int id)
    {
        var success = await _bookingService.CheckOutAsync(id);
        if (success)
            TempData["Success"] = "Guest checked out successfully";
        else
            TempData["Error"] = "Unable to check out guest. Booking must be in CheckedIn status.";

        return RedirectToAction("Index");
    }
}
