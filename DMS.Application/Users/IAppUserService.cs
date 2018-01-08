using DMS.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Users
{
  public interface IAppUserService
  {
    Task<UserFullDto> GetUser(int id);
  }
}
