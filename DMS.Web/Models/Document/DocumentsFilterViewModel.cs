using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Models.Document
{
  public class DocumentsFilterViewModel
  {
    public int? AuthorId { get; set; }

    public bool RequireAttention { get; set; }

    public DocumentStatus? Status  { get; set; }

    [StringLength(100)]
    public string SearchString  { get; set; }
  }
}
