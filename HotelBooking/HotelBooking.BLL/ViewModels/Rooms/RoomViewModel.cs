using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.BLL.ViewModels.Rooms;

public class RoomViewModel
{
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Room Number is required")]
    [Display(Name = "Room Number")]
    [StringLength(10, ErrorMessage = "Room Number cannot be longer than 10 characters")]
    public string RoomNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Room Type is required")]
    [Display(Name = "Room Type")]
    public int RoomTypeId { get; set; }

    public string? RoomTypeName { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Available";

    // For dropdown list
    public IEnumerable<SelectListItem>? RoomTypes { get; set; }
}
