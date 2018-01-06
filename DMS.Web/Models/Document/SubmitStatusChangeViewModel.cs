using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Document
{
  public class SubmitStatusChangeViewModel
  {
    public int DocumentId { get; set; }
    [StringLength(255, MinimumLength = 0)]
    public string Message { get; set; }
  }
}
