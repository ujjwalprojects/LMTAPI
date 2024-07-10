using LMT.Application.DTOs;

namespace LMT.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> AuthenticateUserAsync(LoginRequest loginRequest);
        Task RegisterAsync(RegisterRequest registerRequest);
        Task<TokenResponse> RefreshToken(TokenModel tokenModel);
        Task RevokeAsync(string refreshToken);
        Task RevokeAllAsync();
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);
    }
}
