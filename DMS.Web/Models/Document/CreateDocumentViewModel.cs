using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Document
{
  public class CreateDocumentViewModel
  {
    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string Title { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 1)]
    public string Body { get; set; }
  }
}
