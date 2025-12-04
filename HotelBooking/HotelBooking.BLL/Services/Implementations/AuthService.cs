using HotelBooking.DAL.Models;
using HotelBooking.DAL.Repositories;
using HotelBooking.BLL.Services.Interfaces;
using HotelBooking.BLL.ViewModels;

namespace HotelBooking.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public async Task<User> RegisterAsync(RegisterViewModel model)
    {
        if (await _userRepository.EmailExistsAsync(model.Email))
            throw new InvalidOperationException("Email already exists");

        var user = new User
        {
            Email = model.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            Role = UserRole.Customer,
            CreatedAt = DateTime.Now
        };

        return await _userRepository.AddAsync(user);
    }
}
