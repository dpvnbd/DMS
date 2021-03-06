﻿using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Users
{
  public class UserFullDto
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; }

    public bool CanCreateOnBehalfOfUser { get; set; }

  }
}
