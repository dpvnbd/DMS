using System.Security.Claims;
using System.Threading.Tasks;
using DMS.Application.DTOs.Authentication;
using DMS.Application.DTOs.Users;
using DMS.Infrastructure.Data.Identity;

namespace DMS.Application.Authentication
{
  public interface IAuthService
  {
    Task<bool> Login(LoginDto loginDTO);
    Task<bool> Register(RegisterDto registerDTO);
    Task Logout();
    Task<bool> ChangePassword(ClaimsPrincipal user, string oldPassword, string newPassword);

    Task<int> GetUserIdByClaims(ClaimsPrincipal user);

    Task<UserSummaryDto> GetUserByClaims(ClaimsPrincipal user);
  }
}