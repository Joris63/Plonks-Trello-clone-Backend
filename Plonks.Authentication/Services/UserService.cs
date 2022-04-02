﻿using Microsoft.EntityFrameworkCore;
using Plonks.Auth.Entities;
using Plonks.Auth.Helpers;
using Plonks.Auth.Models;

namespace Plonks.Auth.Services
{
    public interface IUserService
    {
        Task<EditUserResponse> Edit(EditUserRequest model);
        Task<EditUserResponse> ChangePassword(ChangePasswordRequest model);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IUserMethods _userMethods;

        public UserService(AppDbContext context, IUserMethods userMethods)
        {
            _context = context;
            _userMethods = userMethods;
        }

        public async Task<EditUserResponse> Edit(EditUserRequest model)
        {
            User? retrievedUser = retrievedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
            string message = "No user found.";

            if(retrievedUser == null)
            {
                return new EditUserResponse(message);
            }

            if (model.PicturePath != null)
            {
                retrievedUser.PicturePath = model.PicturePath;
                message = "Profile picture updated.";
            }

            if (model.Username != null)
            {
                retrievedUser.Username = model.Username;
                message = "Profile updated.";
            }

            if(model.Email != null && !retrievedUser.SocialAccount)
            {
                if (await EmailExists(model.Email))
                {
                    message = "Email is already registered.";
                    return new EditUserResponse(message);
                }

                retrievedUser.Email = model.Email;
                message = "Profile updated.";
            }

            string token = _userMethods.CreateAccessToken(retrievedUser);

            await _context.SaveChangesAsync();

            return new EditUserResponse(retrievedUser, token, message);
        }

        public async Task<EditUserResponse> ChangePassword(ChangePasswordRequest model)
        {
            User? retrievedUser = retrievedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
            string message = "No user found.";

            if (retrievedUser == null)
            {
                return new EditUserResponse(message);
            }

            if (!_userMethods.VerifyPasswordHash(model.OldPassword, retrievedUser.PasswordHash, retrievedUser.PasswordSalt))
            {
                message = "Incorrect password.";
                return new EditUserResponse(message);
            }

            _userMethods.CreatePasswordHash(model.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            retrievedUser.PasswordHash = passwordHash;
            retrievedUser.PasswordSalt = passwordSalt;

            string token = _userMethods.CreateAccessToken(retrievedUser);
            await _context.SaveChangesAsync();

            message = "Password updated.";

            return new EditUserResponse(retrievedUser, token, message);
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
    }
}
