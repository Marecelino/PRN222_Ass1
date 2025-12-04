using HotelBooking.DAL.Models;
using HotelBooking.BLL.ViewModels;

namespace HotelBooking.BLL.Services;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User> RegisterAsync(RegisterViewModel model);
}
