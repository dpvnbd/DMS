using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Account
{
  public class RegisterViewModel
  {
    [Display(Name = "Email")]
    [Required]
    [DataType(DataType.EmailAddress)]    
    public string Email { get; set; }

    [Display(Name = "Username")]
    [Required]
    public string UserName { get; set; }

    [Display(Name = "First Name")]
    [Required]
    [StringLength(maximumLength: 20, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    [Required]
    [StringLength(maximumLength: 20, MinimumLength = 2)]
    public string LastName { get; set; }

    [Display(Name = "Password")]
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm Password")]
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
  }
}
