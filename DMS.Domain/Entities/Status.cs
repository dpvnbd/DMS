using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities
{
  /// <summary>
  /// The stages of document review 
  /// </summary>
  public enum DocumentStatus
  {
    /// <summary>
    /// Customer created a new document
    /// </summary>
    Created,

    /// <summary>
    /// Customer created or edited document and submitted it for review
    /// </summary>
    Submitted,

    /// <summary>
    /// Operator reviewed a document and sent it back for fixing errors
    /// </summary>
    Rejected,

    /// <summary>
    /// Operator reviewed a document and sent it further for expert review
    /// </summary>
    Approved,

    /// <summary>
    /// Expert reviewed a document and accepted the request
    /// </summary>
    Accepted,

    /// <summary>
    /// Expert reviewed a document and declined the request
    /// </summary>
    Declined
  }
}
