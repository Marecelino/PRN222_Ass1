using HotelBooking.DAL.Models;
using System.Collections.Generic;

namespace HotelBooking.BLL.ViewModels;

public class RoomDetailsViewModel
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
    public double AverageRating { get; set; }
}
