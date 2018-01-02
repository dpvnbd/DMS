using DMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  public class AppUser : BaseEntity
  {
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public UserRole Role { get; protected set; }

    protected AppUser() { } // Empty constructor for EF

    public AppUser(string firstName, string lastName, UserRole role)
    {
      if (string.IsNullOrWhiteSpace(firstName))
      {
        throw new ArgumentException("Name is empty", nameof(firstName));
      }

      if (string.IsNullOrWhiteSpace(lastName))
      {
        throw new ArgumentException("Name is empty", nameof(lastName));
      }

      FirstName = firstName;
      LastName = lastName;
      Role = role;
    }
  }
}
