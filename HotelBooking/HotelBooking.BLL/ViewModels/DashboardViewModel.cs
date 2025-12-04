namespace HotelBooking.BLL.ViewModels;

public class DashboardViewModel
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
    public List<BookingsByRoomType> BookingsByRoomType { get; set; } = new();
    public List<DailyRevenue> DailyRevenues { get; set; } = new();
}

public class BookingsByRoomType
{
    public string RoomTypeName { get; set; } = string.Empty;
    public int BookingCount { get; set; }
}

public class DailyRevenue
{
    public string Date { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}
