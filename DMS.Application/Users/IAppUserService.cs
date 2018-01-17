using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Users
{
  public interface IAppUserService
  {
    Task<UserFullDto> GetUser(int id, int requestingUserId = -1);

    Task<UserSummaryDto> GetUserSummary(int id, int requestingUserId = -1);

    Task<IEnumerable<UserSummaryDto>> FindUsers(Func<AppUser, bool> predicate, int requestingUserId = -1);

    Task<bool> SetRole(int userId, UserRole role);   
  }
}
