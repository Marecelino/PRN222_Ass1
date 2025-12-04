using HotelBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.DAL.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context, IConfiguration configuration)
    {
        // Ensure database is created
        context.Database.Migrate();

        // Check if already seeded
        if (context.Users.Any())
        {
            return; // DB has been seeded
        }

        // Seed Admin from appsettings.json
        var adminConfig = configuration.GetSection("SeedAdmin");
        var adminEmail = adminConfig["Email"] ?? "admin@hotel.com";
        var adminPassword = adminConfig["Password"] ?? "Admin@123";
        var adminFullName = adminConfig["FullName"] ?? "System Administrator";

        var adminUser = new User
        {
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
            FullName = adminFullName,
            PhoneNumber = "0123456789",
            Role = UserRole.Admin,
            CreatedAt = DateTime.Now
        };

        // Seed Staff
        var staffUser = new User
        {
            Email = "staff@hotel.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
            FullName = "Hotel Staff",
            PhoneNumber = "0123456788",
            Role = UserRole.Staff,
            CreatedAt = DateTime.Now
        };

        // Seed Customer
        var customerUser = new User
        {
            Email = "customer@hotel.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
            FullName = "John Customer",
            PhoneNumber = "0123456787",
            Role = UserRole.Customer,
            CreatedAt = DateTime.Now
        };

        context.Users.AddRange(adminUser, staffUser, customerUser);
        context.SaveChanges();

        // Seed Room Types
        var roomTypes = new[]
        {
            new RoomType
            {
                TypeName = "Standard Single",
                Description = "Comfortable single room with basic amenities",
                MaxOccupancy = 1,
                PricePerNight = 50.00m
            },
            new RoomType
            {
                TypeName = "Standard Double",
                Description = "Spacious double room perfect for couples",
                MaxOccupancy = 2,
                PricePerNight = 80.00m
            },
            new RoomType
            {
                TypeName = "Deluxe Suite",
                Description = "Luxurious suite with separate living area",
                MaxOccupancy = 4,
                PricePerNight = 150.00m
            },
            new RoomType
            {
                TypeName = "Family Room",
                Description = "Large room suitable for families",
                MaxOccupancy = 5,
                PricePerNight = 120.00m
            }
        };

        context.RoomTypes.AddRange(roomTypes);
        context.SaveChanges();

        // Seed Rooms
        var rooms = new List<Room>();
        var roomTypesList = context.RoomTypes.ToList();

        // Standard Single rooms (101-102)
        for (int i = 1; i <= 2; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"10{i}",
                RoomTypeId = roomTypesList[0].RoomTypeId,
                Status = RoomStatus.Available
            });
        }

        // Standard Double rooms (201-204)
        for (int i = 1; i <= 4; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"20{i}",
                RoomTypeId = roomTypesList[1].RoomTypeId,
                Status = RoomStatus.Available
            });
        }

        // Deluxe Suite (301-302)
        for (int i = 1; i <= 2; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"30{i}",
                RoomTypeId = roomTypesList[2].RoomTypeId,
                Status = RoomStatus.Available
            });
        }

        // Family Room (401-402)
        for (int i = 1; i <= 2; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"40{i}",
                RoomTypeId = roomTypesList[3].RoomTypeId,
                Status = RoomStatus.Available
            });
        }

        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        // Seed sample bookings
        var roomsList = context.Rooms.Include(r => r.RoomType).ToList();
        var customer = context.Users.First(u => u.Role == UserRole.Customer);

        var bookings = new[]
        {
            new Booking
            {
                UserId = customer.UserId,
                RoomId = roomsList[0].RoomId,
                CheckInDate = DateTime.Today.AddDays(-5),
                CheckOutDate = DateTime.Today.AddDays(-3),
                NumberOfGuests = 1,
                Status = BookingStatus.CheckedOut,
                TotalPrice = roomsList[0].RoomType.PricePerNight * 2,
                BookingDate = DateTime.Today.AddDays(-7),
                ActualCheckInTime = DateTime.Today.AddDays(-5).AddHours(14),
                ActualCheckOutTime = DateTime.Today.AddDays(-3).AddHours(11)
            },
            new Booking
            {
                UserId = customer.UserId,
                RoomId = roomsList[2].RoomId,
                CheckInDate = DateTime.Today.AddDays(-2),
                CheckOutDate = DateTime.Today.AddDays(1),
                NumberOfGuests = 2,
                Status = BookingStatus.CheckedIn,
                TotalPrice = roomsList[2].RoomType.PricePerNight * 3,
                BookingDate = DateTime.Today.AddDays(-4),
                ActualCheckInTime = DateTime.Today.AddDays(-2).AddHours(15)
            },
            new Booking
            {
                UserId = customer.UserId,
                RoomId = roomsList[4].RoomId,
                CheckInDate = DateTime.Today.AddDays(3),
                CheckOutDate = DateTime.Today.AddDays(5),
                NumberOfGuests = 2,
                Status = BookingStatus.Confirmed,
                TotalPrice = roomsList[4].RoomType.PricePerNight * 2,
                BookingDate = DateTime.Today
            }
        };

        context.Bookings.AddRange(bookings);
        context.SaveChanges();
    }
}
