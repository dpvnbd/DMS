using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Account
{
  public class ChangePasswordViewModel
  {
    [Display(Name = "Current Password")]
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [Display(Name = "New Password")]
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Display(Name = "Confirm New Password")]
    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; }
  }
}
