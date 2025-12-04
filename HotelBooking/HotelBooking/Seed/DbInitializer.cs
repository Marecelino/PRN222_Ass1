using HotelBooking.DAL.Data;

using HotelBooking.DAL.Models;

namespace HotelBooking.Seed;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Create database if not exists (no migrations required for now)
        context.Database.EnsureCreated();

        // Seed users
        if (!context.Users.Any())
        {
            var admin = new User
            {
                Email = "admin@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "System Admin",
                PhoneNumber = "0000000000",
                Role = UserRole.Admin,
                CreatedAt = DateTime.Now
            };

            var staff = new User
            {
                Email = "staff@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                FullName = "Front Desk Staff",
                PhoneNumber = "0000000001",
                Role = UserRole.Staff,
                CreatedAt = DateTime.Now
            };

            var customer = new User
            {
                Email = "customer@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                FullName = "Demo Customer",
                PhoneNumber = "0000000002",
                Role = UserRole.Customer,
                CreatedAt = DateTime.Now
            };

            context.Users.AddRange(admin, staff, customer);
            context.SaveChanges();
        }

        // Seed room types
        if (!context.RoomTypes.Any())
        {
            var standard = new RoomType
            {
                TypeName = "Standard",
                Description = "Cozy standard room",
                MaxOccupancy = 2,
                PricePerNight = 80
            };
            var deluxe = new RoomType
            {
                TypeName = "Deluxe",
                Description = "Spacious deluxe room",
                MaxOccupancy = 3,
                PricePerNight = 120
            };
            var suite = new RoomType
            {
                TypeName = "Suite",
                Description = "Luxury suite with living area",
                MaxOccupancy = 4,
                PricePerNight = 200
            };
            var family = new RoomType
            {
                TypeName = "Family",
                Description = "Family room for groups",
                MaxOccupancy = 5,
                PricePerNight = 150
            };

            context.RoomTypes.AddRange(standard, deluxe, suite, family);
            context.SaveChanges();
        }

        // Seed rooms
        if (!context.Rooms.Any())
        {
            var types = context.RoomTypes.ToList();

            var rooms = new List<Room>
            {
                new() { RoomNumber = "101", RoomTypeId = types[0].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "102", RoomTypeId = types[0].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "201", RoomTypeId = types[1].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "202", RoomTypeId = types[1].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "301", RoomTypeId = types[2].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "302", RoomTypeId = types[2].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "401", RoomTypeId = types[3].RoomTypeId, Status = RoomStatus.Available },
                new() { RoomNumber = "402", RoomTypeId = types[3].RoomTypeId, Status = RoomStatus.Available },
            };

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }

        // Seed a few sample bookings
        if (!context.Bookings.Any())
        {
            var customer = context.Users.First(u => u.Role == UserRole.Customer);
            var someRoom = context.Rooms.First();

            var today = DateTime.Today;
            var booking1 = new Booking
            {
                UserId = customer.UserId,
                RoomId = someRoom.RoomId,
                CheckInDate = today.AddDays(1),
                CheckOutDate = today.AddDays(3),
                NumberOfGuests = 2,
                Status = BookingStatus.Confirmed,
                TotalPrice = 2 * someRoom.RoomType!.PricePerNight,
                BookingDate = DateTime.Now
            };

            context.Bookings.Add(booking1);
            context.SaveChanges();
        }
    }
}
