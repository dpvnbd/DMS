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

    private List<DocumentHistoryEntry> history = new List<DocumentHistoryEntry>();
    public IReadOnlyCollection<DocumentHistoryEntry> History => history;

    public string Title { get; protected set; }
    public string Body { get; protected set; }

    private List<string> attachments = new List<string>();
    public IReadOnlyCollection<string> Attachments => attachments;


    public DocumentHistoryEntry LastHistoryEntry
    {
      get
      {
        if (history == null || !history.Any())
        {
          return null;
        }
        return history[history.Count - 1];
      }
    }

    public DocumentStatus CurrentStatus { get; protected set; }

    //Empty constructor for EF
    protected Document() { }

    public Document(string title, string body, AppUser author, string message = null, AppUser onBehalfOfUser = null)
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
      history.Add(new DocumentHistoryEntry(author, DocumentStatus.Created,
        message ?? string.Empty, Created, onBehalfOfUser));
    }

    public IEnumerable<DocumentStatus> AvailableStatusChanges(AppUser user)
    {
      var list = new List<DocumentStatus>();

      var isAuthor = user.Id == Author.Id;
      var isOperator = user.Role == UserRole.Operator;
      var isExpert = user.Role == UserRole.Expert;


      if (isOperator)
      {
        if (CurrentStatus == DocumentStatus.Created || CurrentStatus == DocumentStatus.Resubmitted)
        {
          list.Add(DocumentStatus.Registered);
          list.Add(DocumentStatus.Rejected);
          list.Add(DocumentStatus.Canceled);
        }
      }

      if (isExpert)
      {
        if (CurrentStatus == DocumentStatus.Registered)
        {
          list.Add(DocumentStatus.Approved);
          list.Add(DocumentStatus.Canceled);
        }
        else if (CurrentStatus == DocumentStatus.Approved)
        {
          list.Add(DocumentStatus.Done);
        }
      }

      if (CurrentStatus == DocumentStatus.Rejected && isAuthor)
      {
        // Allow user to resubmit only if he recently edited the document
        if(LastHistoryEntry.User.Id == user.Id && !LastHistoryEntry.Status.HasValue)
        {
          list.Add(DocumentStatus.Resubmitted);
        }
      }

      return list;
    }

    public void ChangeStatus(AppUser changeAuthor, DocumentStatus newStatus, string message)
    {
      var availableChanges = AvailableStatusChanges(changeAuthor);

      var canChange = availableChanges.Contains(newStatus);

      if (!canChange)
      {
        throw new InvalidOperationException("Can't change document status");
      }

      if (message == null)
      {
        message = string.Empty;
      }

      history.Add(new DocumentHistoryEntry(changeAuthor, newStatus, message, DateTime.Now));

      CurrentStatus = newStatus;
    }

    public bool CanEdit(AppUser user)
    {
      var isAuthor = user.Id == Author.Id;
      var isOperator = user.Role == UserRole.Operator;
      var isExpert = user.Role == UserRole.Expert;

      if (isAuthor && CurrentStatus == DocumentStatus.Rejected)
      {
        return true;
      }

      if (isOperator && (CurrentStatus == DocumentStatus.Created || CurrentStatus == DocumentStatus.Resubmitted))
      {
        return true;
      }

      if (isExpert && CurrentStatus == DocumentStatus.Registered)
      {
        return true;
      }

      return false;
    }

    public void Edit(AppUser editAuthor, string title, string body, string message)
    {
      var canEdit = CanEdit(editAuthor);

      if (!canEdit)
      {
        throw new InvalidOperationException("The document can be edited only by Expert or by Author before submition");
      }

      if (string.IsNullOrWhiteSpace(title))
      {
        throw new ArgumentException("Title can't be empty", nameof(title));
      }

      if (string.IsNullOrWhiteSpace(body))
      {
        throw new ArgumentException("Body can't be empty", nameof(body));
      }

      if (message == null)
      {
        message = string.Empty;
      }

      Title = title;
      Body = body;
      Modified = DateTime.Now;

      history.Add(new DocumentHistoryEntry(editAuthor, null, message, Modified));
    }
  }
}
