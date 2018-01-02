using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DMS.Domain.Abstract;

namespace DMS.Domain.Entities
{
  public class Document : BaseEntity //:IAggregateRoot
  {
    public DateTime Created { get; protected set; }
    public DateTime Modified { get; protected set; }
    public AppUser Author { get; protected set; }

    private List<StatusChange> _statusChanges = new List<StatusChange>();
    public IReadOnlyCollection<StatusChange> StatusChanges => _statusChanges;

    public string Title { get; protected set; }
    public string Body { get; protected set; }

    //Empty constructor for EF
    protected Document() { }

    public Document(string title, string body, AppUser author)
    {
      if (string.IsNullOrWhiteSpace(title))
      {
        throw new ArgumentException("Title can't be empty", nameof(title));
      }

      if (string.IsNullOrWhiteSpace(body))
      {
        throw new ArgumentException("Body can't be empty", nameof(body));
      }

      Author = author ?? throw new ArgumentNullException(nameof(author));
      Title = title;
      Body = body;
      Created = DateTime.Now;
      Modified = Created;

      ChangeStatus(author, DocumentStatus.Created, "Document is created", Created);
    }

    public StatusChange GetLastStatus()
    {
      if (!_statusChanges.Any())
      {
        return null;
      }
      return _statusChanges[_statusChanges.Count - 1];
    }

    public void ChangeStatus(AppUser changeAuthor, DocumentStatus newStatus, string message, DateTime changeDateTime)
    {
      var lastStatusChange = GetLastStatus();
      DocumentStatus currentStatus;
      if (lastStatusChange == null && newStatus != DocumentStatus.Created)
      {
        throw new InvalidOperationException("Document must have status \"Created\" first");
      }
      else if (lastStatusChange != null && newStatus == DocumentStatus.Created)
      {
        throw new InvalidOperationException("Document can only have \"Created\" status once");
      }
      else if (lastStatusChange == null)
      {
        currentStatus = DocumentStatus.Created;
      }
      else
      {
        currentStatus = GetLastStatus().Status;
      }

      if (newStatus == DocumentStatus.Submitted || newStatus == DocumentStatus.Created
        && Author.Id != changeAuthor.Id)
      {
        throw new InvalidOperationException("Only document author can create or submit it");
      }

      if (newStatus == DocumentStatus.Submitted
        && (currentStatus != DocumentStatus.Rejected || currentStatus != DocumentStatus.Created))
      {
        throw new InvalidOperationException("Document can only be submitted when just created or rejected by operator");
      }

      if ((newStatus == DocumentStatus.Rejected || newStatus == DocumentStatus.Approved)
        && currentStatus != DocumentStatus.Submitted)
      {
        throw new InvalidOperationException("Document can only be reviewed by operator when submitted");
      }

      if ((newStatus == DocumentStatus.Declined || newStatus == DocumentStatus.Accepted)
        && currentStatus != DocumentStatus.Approved)
      {
        throw new InvalidOperationException("Document can only be reviewed by expert after approved by operator");
      }

      _statusChanges.Add(new StatusChange(changeAuthor, newStatus, message, changeDateTime));
    }


    public void EditDocument(AppUser editAuthor, string title, string body)
    {
      if (string.IsNullOrWhiteSpace(title))
      {
        throw new ArgumentException("Title can't be empty", nameof(title));
      }

      if (string.IsNullOrWhiteSpace(body))
      {
        throw new ArgumentException("Body can't be empty", nameof(body));
      }

      if (editAuthor.Id != Author.Id && editAuthor.Role != UserRole.Expert)
      {
        throw new ArgumentException("Only author or experts can edit the document", nameof(editAuthor));
      }

      var lastStatus = GetLastStatus();
      if (lastStatus == null || lastStatus.Status == DocumentStatus.Created || lastStatus.Status == DocumentStatus.Rejected)
      {
        throw new InvalidOperationException("Only just created or rejected documents can be edited");
      }

      Title = title;
      Body = body;
      Modified = DateTime.Now;
    }
  }
}
