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

    public IEnumerable<UserSummaryDto> FindUsers(Func<AppUser, bool> predicate)
    {
      var users = userRepo.GetAll().Where(predicate);
      var summaries = mapper.Map<IEnumerable<UserSummaryDto>>(users);
      return summaries;
    }

    public async Task<UserFullDto> GetUser(int id)
    {
      var user = await userRepo.GetById(id);
      var dto = mapper.Map<UserFullDto>(user);
      return dto;
    }
  }
}
