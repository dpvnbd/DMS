using DMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  public class ApplicationUser : BaseEntity
  {
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public UserRole Role { get; protected set; }

    protected ApplicationUser() { } // Empty constructor for EF

    public ApplicationUser(string firstName, string lastName, UserRole role)
    {
      if (string.IsNullOrWhiteSpace(firstName))
      {
        throw new ArgumentException("Name is empty", nameof(firstName));
      }

      if (string.IsNullOrWhiteSpace(lastName))
      {
        throw new ArgumentException("Name is empty", nameof(lastName));
      }

      Role = role;
    }
  }
}
