using System.Threading.Tasks;
using DMS.Application.DTOs.Authentication;

namespace DMS.Application.Authentication
{
  public interface IAuthService
  {
    Task<bool> Login(LoginDto loginDTO);
    Task<bool> Register(RegisterDto registerDTO);
    Task Logout();
  }
}