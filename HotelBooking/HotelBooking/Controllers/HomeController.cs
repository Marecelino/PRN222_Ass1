using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HotelBooking.DAL.Models;
using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

public class HomeController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;
    private readonly IReviewService _reviewService;

    public HomeController(IBookingService bookingService, IRoomService roomService, IReviewService reviewService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
        _reviewService = reviewService;
    }

    public async Task<IActionResult> Details(int id)
    {
        var room = await _roomService.GetRoomAsync(id);
        if (room == null) return NotFound();

        var reviews = await _reviewService.GetRoomReviewsAsync(id);
        var avgRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

        var model = new RoomDetailsViewModel
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            TypeName = room.RoomType.TypeName,
            Description = room.RoomType.Description,
            PricePerNight = room.RoomType.PricePerNight,
            MaxOccupancy = room.RoomType.MaxOccupancy,
            Reviews = reviews,
            AverageRating = avgRating
        };

        return View(model);
    }

    public IActionResult Index()
    {
        var model = new SearchViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Search(SearchViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        if (model.CheckOutDate <= model.CheckInDate)
        {
            ModelState.AddModelError(string.Empty, "Check-out date must be after check-in date");
            return View("Index", model);
        }

        try
        {
            var results = await _bookingService.SearchAvailableRoomsAsync(
                model.CheckInDate, model.CheckOutDate, model.NumberOfGuests);
            return View("SearchResults", results);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Index", model);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
