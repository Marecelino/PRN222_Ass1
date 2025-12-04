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

    public HomeController(IBookingService bookingService)
    {
        _bookingService = bookingService;
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
