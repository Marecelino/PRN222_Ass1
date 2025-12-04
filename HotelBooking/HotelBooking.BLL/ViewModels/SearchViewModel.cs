namespace HotelBooking.BLL.ViewModels;

public class SearchViewModel
{
    public DateTime CheckInDate { get; set; } = DateTime.Today.AddDays(1);
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(2);
    public int NumberOfGuests { get; set; } = 1;
}
