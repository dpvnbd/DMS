using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  /// <summary>
  /// Roles of users
  /// </summary>
    public enum UserRole
    {
    /// <summary>
    /// A user that can only submit documents
    /// </summary>
    Customer,

    /// <summary>
    /// A user that can review documents for errors and send them back to creators for editing or send them to experts
    /// </summary>
    Operator,

    /// <summary>
    /// A user that can review documents to accept or decline requests of users
    /// </summary>
    Expert
  }
}
