using System.ComponentModel.DataAnnotations;

namespace HotelBooking.BLL.ViewModels.RoomTypes;

public class RoomTypeViewModel
{
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "Type Name is required")]
    [Display(Name = "Type Name")]
    [StringLength(50, ErrorMessage = "Type Name cannot be longer than 50 characters")]
    public string TypeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Max Occupancy is required")]
    [Range(1, 20, ErrorMessage = "Max Occupancy must be between 1 and 20")]
    [Display(Name = "Max Occupancy")]
    public int MaxOccupancy { get; set; }

    [Required(ErrorMessage = "Price Per Night is required")]
    [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Price Per Night")]
    [DataType(DataType.Currency)]
    public decimal PricePerNight { get; set; }
}
