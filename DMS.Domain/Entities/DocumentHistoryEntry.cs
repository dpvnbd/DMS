using DMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  public class DocumentHistoryEntry : BaseEntity
  {
    public int UserId { get; protected set; }
    public int? UserActingOnBehalfId { get; protected set; }
    public DateTime Created { get; protected set; }

    /// <summary>
    /// If property is not null it's assumed that Document Status has changed.
    /// Otherwise - Document has been edited
    /// </summary>
    public DocumentStatus? Status { get; protected set; }

    public string Message { get; protected set; }

    // Empty constructor for EF
    protected DocumentHistoryEntry() { }

    public DocumentHistoryEntry(AppUser changeAuthor, DocumentStatus? status,
      string message, DateTime created, AppUser userActingOnBehalf = null)
    {
      UserId = changeAuthor.Id;
      Message = message ?? throw new ArgumentNullException(nameof(message));

      Status = status;
      Created = created;

      if (userActingOnBehalf != null)
      {
        UserActingOnBehalfId = userActingOnBehalf.Id;
      }
    }
  }
}
