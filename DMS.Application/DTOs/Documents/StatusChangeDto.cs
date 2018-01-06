using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.DTOs.Documents
{
  public class StatusChangeDto
  {
    public AppUserDto ChangeAuthor { get; set; }
    public DocumentStatus Status { get; set; }
    public string Message { get; set; }
    public DateTime Created { get; protected set; }
  }
}
