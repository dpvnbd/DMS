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
    /// Document has been created by customer and submitted for Operator review
    /// </summary>
    Created,

    /// <summary>
    /// Operator reviewed a document and rejects it
    /// </summary>
    Rejected,


    /// <summary>
    /// Customer has edited the Rejected document and submits it again for Operator review
    /// </summary>
    Resubmitted,

    /// <summary>
    /// Operator has reviewed a document and sends it further for Expert review
    /// </summary>
    Registered,

    /// <summary>
    /// Expert has reviewed a document and accepts the request
    /// </summary>
    Approved,

    /// <summary>
    /// Document has been discarded during the Operator or Expert review and can't be resubmitted
    /// </summary>
    Canceled,


    /// <summary>
    /// Expert has fulfilled the request
    /// </summary>
    Done
  }
}
