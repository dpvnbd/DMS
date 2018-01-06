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

    public StatusChange GetLastStatusChange()
    {
      if (!statusChanges.Any())
      {
        return null;
      }
      return statusChanges[statusChanges.Count - 1];
    }

    public IEnumerable<DocumentStatus> AvailableStatusChanges(AppUser user)
    {
      var list = new List<DocumentStatus>();
      var status = GetLastStatusChange().Status;
      
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

      var lastStatus = GetLastStatusChange();
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
