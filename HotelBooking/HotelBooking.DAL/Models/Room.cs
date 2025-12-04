namespace HotelBooking.DAL.Models;

public class Room
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public RoomStatus Status { get; set; }
 
    // Navigation
    public RoomType RoomType { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public enum RoomStatus
{
    Available = 0,
    Occupied = 1,
    Maintenance = 2,
    OutOfService = 3
}
