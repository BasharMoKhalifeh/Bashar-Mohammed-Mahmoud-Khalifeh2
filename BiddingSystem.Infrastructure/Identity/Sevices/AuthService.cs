using BiddingSystem.Application.DTOs.UserDtos;
using BiddingSystem.Domain.Enums;
using BiddingSystem.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace BiddingSystem.Infrastructure.Identity.Sevices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<User> userManager,
            IJwtService jwtService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<AuthResult> RegisterAsync(UserRegistrationDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResult { Errors = new[] { "Email already in use" } };
            }

            var newUser = new User(
                request.FirstName,
                request.LastName,
                request.Email,
                request.UserTypeId);

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);
            if (!createdUser.Succeeded)
            {
                return new AuthResult { Errors = createdUser.Errors.Select(e => e.Description) };
            }

            // Assign role based on UserType
            var role = GetRoleForUserType(request.UserTypeId);
            await _userManager.AddToRoleAsync(newUser, role);

            var token = await _jwtService.GenerateTokenAsync(newUser);
            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> LoginAsync(UserLoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResult { Errors = new[] { "Invalid login request" } };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new AuthResult { Errors = new[] { "Invalid login request" } };
            }

            if (!user.IsActive)
            {
                return new AuthResult { Errors = new[] { "Account is deactivated" } };
            }

            user.UpdateLoginTime();
            await _userManager.UpdateAsync(user);

            var token = await _jwtService.GenerateTokenAsync(user);
            return new AuthResult { Success = true, Token = token };
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            return result.Succeeded;
        }

        private string GetRoleForUserType(int userTypeId)
        {
            // This would typically come from database
            return userTypeId switch
            {
                1 => UserRole.Administrator,
                2 => UserRole.ProcurementOfficer,
                3 => UserRole.Bidder,
                4 => UserRole.Evaluator,
                _ => UserRole.Bidder
            };
        }

        public Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}

