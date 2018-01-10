using DMS.Application.Authentication;
using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Account
{
  public class UserRolesViewModel
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public string IdentityId { get; set; }

    [Display(Name = "Reviewer role")]
    [Required]
    public UserRole UserRole { get; set; }

    [Display(Name = "Administration role")]
    [Required]
    public AppUserIdentityRoleEnum IdentityRole { get; set; }
  }
}
