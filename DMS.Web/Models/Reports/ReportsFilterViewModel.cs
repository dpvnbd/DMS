using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Reports
{
  public class ReportsFilterViewModel
  {
    public int? DocumentId { get; set; }

    public int? UserId { get; set; }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
  }
}
