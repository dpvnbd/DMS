using DMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  public class StatusChange : BaseEntity
  {
    public AppUser ChangeAuthor { get; protected set; }
    public Document Document { get; protected set; }
    public DocumentStatus Status { get; protected set; }
    public string Message { get; protected set; }

    public DateTime Created { get; protected set; }
    // Empty constructor for EF
    protected StatusChange() { }

    public StatusChange(AppUser changeAuthor, DocumentStatus newStatus, string message, DateTime created)
    {
      ChangeAuthor = changeAuthor ?? throw new ArgumentNullException(nameof(changeAuthor));
      Message = message ?? throw new ArgumentNullException(nameof(message));

      if (changeAuthor.Role == UserRole.Customer &&
        newStatus != DocumentStatus.Submitted && newStatus != DocumentStatus.Created)
      {
        throw new ArgumentException("Custormer can only create and submit documents", nameof(newStatus));
      }

      if (changeAuthor.Role == UserRole.Operator &&
        (newStatus == DocumentStatus.Declined && newStatus != DocumentStatus.Accepted))
      {
        throw new ArgumentException("Operator can't decline or accept documents", nameof(newStatus));
      }

      Status = newStatus;
      Created = created;
    }
  }
}
