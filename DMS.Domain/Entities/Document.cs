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

    private List<StatusChange> statusChanges = new List<StatusChange>();
    public IReadOnlyCollection<StatusChange> StatusChanges => statusChanges;

    public string Title { get; protected set; }
    public string Body { get; protected set; }

    public StatusChange LastStatusChange
    {
      get
      {
        if (!statusChanges.Any())
        {
          return null;
        }
        return statusChanges[statusChanges.Count - 1];
      }
    }

    public DocumentStatus CurrentDocumentStatus => (DocumentStatus)LastStatusChange?.Status;

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

      statusChanges.Add(new StatusChange(author, DocumentStatus.Created, string.Empty, Created));
    }    

    public IEnumerable<DocumentStatus> AvailableStatusChanges(AppUser user)
    {
      var list = new List<DocumentStatus>();
      var status = LastStatusChange.Status;
      
      var isAuthor = user.Id == Author.Id;
      var isCustomer = user.Role == UserRole.Customer;
      var isOperator = user.Role == UserRole.Operator;
      var isExpert = user.Role == UserRole.Expert;


      if ((status == DocumentStatus.Created || status == DocumentStatus.Rejected) && isAuthor)
      {
        list.Add(DocumentStatus.Submitted);
      }

      if (status == DocumentStatus.Submitted && (isOperator || isExpert))
      {
        list.Add(DocumentStatus.Approved);
        list.Add(DocumentStatus.Rejected);
      }

      if (status == DocumentStatus.Approved && isExpert)
      {
        list.Add(DocumentStatus.Accepted);
        list.Add(DocumentStatus.Declined);
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

      if(message == null)
      {
        message = string.Empty;
      }

      statusChanges.Add(new StatusChange(changeAuthor, newStatus, message, DateTime.Now));
    }

    public bool CanEdit(AppUser user)
    {
      var currentStatus = CurrentDocumentStatus;
      // Author can edit his document before submition
      if (user.Id == Author.Id &&
        (currentStatus == DocumentStatus.Created || currentStatus == DocumentStatus.Rejected))
      {
        return true;
      }

      // Experts can always edit the document
      return user.Role == UserRole.Expert;
    }

    public void Edit(AppUser editAuthor, string title, string body)
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

      Title = title;
      Body = body;
      Modified = DateTime.Now;
    }
  }
}
