using Microsoft.EntityFrameworkCore;
using Plonks.Auth.Entities;
using Plonks.Auth.Helpers;
using Plonks.Auth.Models;
using System.Security.Cryptography;

namespace Plonks.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> Register(RegisterRequest model);
        Task<AuthenticateResponse> Login(LoginRequest model);
        Task<AuthenticateResponse> SocialLogin(SocialLoginRequest model);
        Task<RefreshTokenResponse> RefreshToken(string refreshToken);
        Task<bool> RevokeToken(RevokeTokenRequest model);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IUserMethods _userMethods;

        public AuthService(AppDbContext context, IUserMethods userMethods)
        {
            _context = context;
            _userMethods = userMethods;
        }

        public async Task<AuthenticateResponse> Register(RegisterRequest model)
        {
            if (await EmailExists(model.Email))
            {
                return new AuthenticateResponse("This email is already registered.");
            }

            _userMethods.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User newUser = new User(model.Username, model.Email, passwordHash, passwordSalt);

            string token = _userMethods.CreateAccessToken(newUser);
            string refreshToken = GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new AuthenticateResponse(newUser, token, refreshToken);
        }

        public async Task<AuthenticateResponse> Login(LoginRequest model)
        {
            User? retrievedUser = retrievedUser = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(model.Email.ToLower()) && x.SocialAccount.Equals(false));

            if (retrievedUser == null || !_userMethods.VerifyPasswordHash(model.Password, retrievedUser.PasswordHash, retrievedUser.PasswordSalt))
            {
                return new AuthenticateResponse("Incorrect email or password.");
            }

            string token = _userMethods.CreateAccessToken(retrievedUser);
            string refreshToken = GenerateRefreshToken();

            retrievedUser.RefreshToken = refreshToken;
            retrievedUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();

            return new AuthenticateResponse(retrievedUser, token, refreshToken);
        }

        public async Task<AuthenticateResponse> SocialLogin(SocialLoginRequest model)
        {
            bool emailExists = await EmailExists(model.Email);
            User? user = emailExists ? await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(model.Email.ToLower()) && x.SocialAccount.Equals(true)) : new User(model);

            if (user == null)
            {
                return new AuthenticateResponse("This email is already registered.");
            }

            string token = _userMethods.CreateAccessToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            if (!emailExists)
            {
                await _context.Users.AddAsync(user);
            }
            await _context.SaveChangesAsync();

            return new AuthenticateResponse(user, token, refreshToken, emailExists ? "signedIn" : "registered");
        }

        public async Task<RefreshTokenResponse> RefreshToken(string refreshToken)
        {
            User? retrievedUser = retrievedUser = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken.Equals(refreshToken));

            if (retrievedUser == null || retrievedUser.RefreshToken != refreshToken || retrievedUser.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new RefreshTokenResponse("Invalid refresh token.");
            }

            string newAccessToken = _userMethods.CreateAccessToken(retrievedUser);
            string newRefreshToken = GenerateRefreshToken();

            retrievedUser.RefreshToken = newRefreshToken;
            retrievedUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();

            return new RefreshTokenResponse(newAccessToken, newRefreshToken);
        }

        public async Task<bool> RevokeToken(RevokeTokenRequest model)
        {
            User? retrievedUser = retrievedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(model.UserId));

            if (retrievedUser != null)
            {
                retrievedUser.RefreshToken = null;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /*
         Helper methods 
        */
        public async Task<bool> EmailExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
