using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  public class User
  {
    public Guid Id { get; protected set; }
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public UserRole Role { get; protected set; }

    protected User() { } // Empty constructor for EF

    public User(Guid id, string firstName, string lastName, UserRole role)
    {
      if (string.IsNullOrWhiteSpace(firstName))
      {
        throw new ArgumentException("Name is empty", nameof(firstName));
      }

      if (string.IsNullOrWhiteSpace(lastName))
      {
        throw new ArgumentException("Name is empty", nameof(lastName));
      }

      Id = id;
      Role = role;
    }
  }
}
