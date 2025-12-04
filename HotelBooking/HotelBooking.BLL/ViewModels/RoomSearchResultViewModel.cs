using HotelBooking.DAL.Models;

namespace HotelBooking.BLL.ViewModels;

public class RoomSearchResultViewModel
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxOccupancy { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfNights { get; set; }
    public decimal TotalPrice { get; set; }
}
