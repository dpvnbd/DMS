﻿using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs
{
  public class AppUserDto
  {
    public int Id { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; protected set; }
  }
}
