namespace HotelBooking.BLL.ViewModels.Bookings;

public class CreateBookingViewModel
{
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string? SpecialRequests { get; set; }

    // Display properties
    public string RoomNumber { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
}
