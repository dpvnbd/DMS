using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.DTOs.Users;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;

namespace DMS.Application.Users
{
  public class AppUserService : IAppUserService
  {
    private readonly IRepository<AppUser> userRepo;
    private readonly IMapper mapper;

    public AppUserService(IRepository<AppUser> userRepo, IMapper mapper)
    {
      this.userRepo = userRepo;
      this.mapper = mapper;
    }

    private bool CanCreateOnBehalf(UserRole requestingUserRole, UserRole targetUserRole)
    {
      // TODO - find a place in domain to put authorization logic
      return requestingUserRole == UserRole.Operator && targetUserRole == UserRole.Customer;
    }

    public async Task<IEnumerable<UserSummaryDto>> FindUsers(Func<AppUser, bool> predicate, int requestingUserId = -1)
    {
      var users = userRepo.GetAll().Where(predicate);
      var summaries = mapper.Map<IEnumerable<UserSummaryDto>>(users);

      if (requestingUserId != -1)
      {
        var requestingUser = await userRepo.GetById(requestingUserId);
        if (requestingUser != null)
        {
          summaries = summaries.Select(u =>
            {
              u.CanCreateOnBehalfOfUser = CanCreateOnBehalf(requestingUser.Role, u.Role);
              return u;
            });
        }
      }
      return summaries;
    }

    public async Task<UserFullDto> GetUser(int id, int requestingUserId = -1)
    {
      var user = await userRepo.GetById(id);
      var dto = mapper.Map<UserFullDto>(user);
      if (requestingUserId != -1)
      {
        var requestingUser = await userRepo.GetById(requestingUserId);
        if (requestingUser != null)
        {
          dto.CanCreateOnBehalfOfUser = CanCreateOnBehalf(requestingUser.Role, dto.Role);
        }
      }
      return dto;
    }

    public async Task<UserSummaryDto> GetUserSummary(int id, int requestingUserId = -1)
    {
      var user = await userRepo.GetById(id);
      var dto = mapper.Map<UserSummaryDto>(user);
      if (requestingUserId != -1)
      {
        var requestingUser = await userRepo.GetById(requestingUserId);
        if (requestingUser != null)
        {
          dto.CanCreateOnBehalfOfUser = CanCreateOnBehalf(requestingUser.Role, dto.Role);
        }
      }

      return dto;
    }

    public async Task<bool> SetRole(int userId, UserRole role)
    {
      var user = await userRepo.GetById(userId);
      if (user == null)
      {
        return false;
      }

      try
      {
        user.SetRole(role);
        await userRepo.Update(user);
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }
  }
}
