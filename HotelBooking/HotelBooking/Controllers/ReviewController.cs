using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBooking.Controllers;

[Authorize]
public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IBookingService _bookingService;

    public ReviewController(IReviewService reviewService, IBookingService bookingService)
    {
        _reviewService = reviewService;
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int bookingId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        if (!await _reviewService.CanUserReviewAsync(bookingId, userId))
        {
            TempData["Error"] = "You cannot review this booking.";
            return RedirectToAction("MyBookings", "Booking");
        }

        var booking = await _bookingService.GetBookingDetailsAsync(bookingId);
        ViewBag.Booking = booking;

        return View(new Review { BookingId = bookingId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(Review review)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!await _reviewService.CanUserReviewAsync(review.BookingId, userId))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            var booking = await _bookingService.GetBookingDetailsAsync(review.BookingId);
            ViewBag.Booking = booking;
            return View(review);
        }

        try
        {
            await _reviewService.CreateReviewAsync(review.BookingId, review.Rating, review.Comment);
            TempData["Success"] = "Review submitted successfully!";
            return RedirectToAction("MyBookings", "Booking");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var booking = await _bookingService.GetBookingDetailsAsync(review.BookingId);
            ViewBag.Booking = booking;
            return View(review);
        }
    }
}
