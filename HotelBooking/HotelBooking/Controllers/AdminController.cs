using System;
using System.Threading.Tasks;
using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels.RoomTypes;
using HotelBooking.BLL.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;
    private readonly IRoomTypeService _roomTypeService;

    public AdminController(IBookingService bookingService, IRoomService roomService, IRoomTypeService roomTypeService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
        _roomTypeService = roomTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard(DateTime? fromDate, DateTime? toDate)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today;

        var stats = await _bookingService.GetDashboardStatsAsync(from, to);
        return View(stats);
    }

    // Room Types CRUD
    public async Task<IActionResult> RoomTypes()
    {
        var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
        return View("RoomTypes/Index", roomTypes);
    }

    [HttpGet]
    public IActionResult CreateRoomType()
    {
        return View("RoomTypes/Create");
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoomType(RoomTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _roomTypeService.CreateRoomTypeAsync(model);
            TempData["Success"] = "Room Type created successfully";
            return RedirectToAction(nameof(RoomTypes));
        }
        return View("RoomTypes/Create", model);
    }

    [HttpGet]
    public async Task<IActionResult> EditRoomType(int id)
    {
        var roomType = await _roomTypeService.GetRoomTypeByIdAsync(id);
        if (roomType == null) return NotFound();

        var model = new RoomTypeViewModel
        {
            RoomTypeId = roomType.RoomTypeId,
            TypeName = roomType.TypeName,
            Description = roomType.Description,
            MaxOccupancy = roomType.MaxOccupancy,
            PricePerNight = roomType.PricePerNight
        };
        return View("RoomTypes/Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRoomType(RoomTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _roomTypeService.UpdateRoomTypeAsync(model);
            TempData["Success"] = "Room Type updated successfully";
            return RedirectToAction(nameof(RoomTypes));
        }
        return View("RoomTypes/Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRoomType(int id)
    {
        await _roomTypeService.DeleteRoomTypeAsync(id);
        TempData["Success"] = "Room Type deleted successfully";
        return RedirectToAction(nameof(RoomTypes));
    }

    // Rooms CRUD
    public async Task<IActionResult> Rooms()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return View("Rooms/Index", rooms);
    }

    [HttpGet]
    public async Task<IActionResult> CreateRoom()
    {
        var model = new RoomViewModel
        {
            RoomTypes = await GetRoomTypesSelectList()
        };
        return View("Rooms/Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(RoomViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _roomService.CreateRoomAsync(model);
            TempData["Success"] = "Room created successfully";
            return RedirectToAction(nameof(Rooms));
        }
        model.RoomTypes = await GetRoomTypesSelectList();
        return View("Rooms/Create", model);
    }

    [HttpGet]
    public async Task<IActionResult> EditRoom(int id)
    {
        var room = await _roomService.GetRoomAsync(id);
        if (room == null) return NotFound();

        var model = new RoomViewModel
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            RoomTypeId = room.RoomTypeId,
            Status = room.Status.ToString(),
            RoomTypes = await GetRoomTypesSelectList()
        };
        return View("Rooms/Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRoom(RoomViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _roomService.UpdateRoomAsync(model);
            TempData["Success"] = "Room updated successfully";
            return RedirectToAction(nameof(Rooms));
        }
        model.RoomTypes = await GetRoomTypesSelectList();
        return View("Rooms/Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        await _roomService.DeleteRoomAsync(id);
        TempData["Success"] = "Room deleted successfully";
        return RedirectToAction(nameof(Rooms));
    }

    private async Task<IEnumerable<SelectListItem>> GetRoomTypesSelectList()
    {
        var types = await _roomTypeService.GetAllRoomTypesAsync();
        return types.Select(t => new SelectListItem
        {
            Value = t.RoomTypeId.ToString(),
            Text = t.TypeName
        });
    }
}
