using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Users
{
  public class UserIdentityDto
  {
    public UserSummaryDto AppUser { get; set; }
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public IEnumerable<string> IdentityRoles { get; set; }
  }
}
