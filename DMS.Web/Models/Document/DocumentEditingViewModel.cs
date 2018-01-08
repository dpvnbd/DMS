using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Document
{
  public class DocumentEditingViewModel
  {
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string Title { get; set; }

    [Required]
    [StringLength(1023, MinimumLength = 3)]
    public string Body { get; set; }
  }
}
