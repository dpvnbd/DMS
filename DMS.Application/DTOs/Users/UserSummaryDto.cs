using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Users
{
  public class UserSummaryDto
  {
    public int Id { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; protected set; }
  }
}
