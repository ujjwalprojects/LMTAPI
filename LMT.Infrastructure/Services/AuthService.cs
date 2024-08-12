﻿using LMT.Application.DTOs;
using LMT.Application.Interfaces;
using LMT.Domain.Entities;
using LMT.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace LMT.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;

        public AuthService(UserManager<IdentityUser> userManager
                       , SignInManager<IdentityUser> signInManager
                       , RoleManager<IdentityRole> roleManager
                       , IConfiguration configuration
                       , AppDBContext dbContext
                       ,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new IdentityUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                PhoneNumberConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Registration failed");
            }

            if (!await _roleManager.RoleExistsAsync("ADMIN"))
            {
                await _roleManager.CreateAsync(new IdentityRole("ADMIN"));
            }

            await _userManager.AddToRoleAsync(user, "ADMIN");
        }
        public async Task<TokenResponse> AuthenticateUserAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var authClaims = await GenerateClaimsAsync(user);
                var token = GenerateJwtToken(authClaims);
                var refreshToken = GenerateRefreshToken();
                var userRoles = await _userManager.GetRolesAsync(user);

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                var newRefreshTokenModel = new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.Id,
                    ExpiryDate = DateTime.UtcNow.AddDays(refreshTokenValidityInDays),
                    IsRevoked = false
                };

                _dbContext.RefreshTokens.Add(newRefreshTokenModel);
                await _dbContext.SaveChangesAsync();

                return new TokenResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    role = userRoles,
                    expiration = token.ValidTo,
                    profileName = user.UserName,
                    email = user.Email,
                    userName = user.UserName,
                    userId = user.Id
                };
            }
            return null;
        }
        private async Task<List<Claim>> GenerateClaimsAsync(IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return authClaims;
        }
        private JwtSecurityToken GenerateJwtToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:TokenValidityInMinutes"]!)),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<TokenResponse> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return null;
            }

            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken!);
            if (principal == null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            var oldRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenModel.RefreshToken);

            if (oldRefreshToken == null || oldRefreshToken.UserId != user.Id || oldRefreshToken.ExpiryDate <= DateTime.Now || oldRefreshToken.IsRevoked)
            {
                return null;
            }

            var newAccessToken = GenerateJwtToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            oldRefreshToken.IsRevoked = true; // Revoke the old refresh token
            _dbContext.RefreshTokens.Update(oldRefreshToken);

            var newRefreshTokenModel = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!)),
                IsRevoked = false
            };
            _dbContext.RefreshTokens.Add(newRefreshTokenModel);
            await _dbContext.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }
        public async Task RevokeAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (token == null)
            {
                throw new Exception("Token not found");
            }

            token.IsRevoked = true;
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RevokeAllAsync()
        {
            var refreshTokens = await _dbContext.RefreshTokens.ToListAsync();
            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.IsRevoked = true;
            }
            _dbContext.RefreshTokens.UpdateRange(refreshTokens);
            await _dbContext.SaveChangesAsync();
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(changePasswordRequest.UserId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);
            return result.Succeeded;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.NewPassword);
            return result.Succeeded;
        }
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_configuration["Url:DomainUrl"]}/reset-password?token={token}&email={email}";

            // Send email
            var mailMessage = new MailMessage(_configuration["EmailSettings:SenderEmail"], email)
            {
                Subject = "Password Reset",
                Body = $"Please reset your password using this link: {resetLink}",
                IsBodyHtml = true
            };

            using (var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"])))
            {
                smtpClient.Credentials = new NetworkCredential(_configuration["EmailSettings:UserName"], _configuration["EmailSettings:Password"]);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }

            return true;
        }
        public async Task<List<UserListDTO>> GetUserListAsync()
        {
            var users = await _dbContext.Users.ToListAsync(); // Assuming your DbContext has a DbSet<ApplicationUser> Users
            return _mapper.Map<List<UserListDTO>>(users);
        }
    }
}
