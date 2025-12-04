namespace HotelBooking.DAL.Models;

public class RoomType
{
    public int RoomTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxOccupancy { get; set; }
    public decimal PricePerNight { get; set; }

    // Navigation
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
