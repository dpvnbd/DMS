using DMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities.Reporting
{
  public class StatusChange : BaseValueObject
  {
    public StatusChange(DocumentStatus fromStatus, DocumentStatus toStatus)
    {
      FromStatus = fromStatus;
      ToStatus = toStatus;
    }
    public DocumentStatus FromStatus { get; }
    public DocumentStatus ToStatus { get; }

    
    protected override IEnumerable<object> GetAtomicValues()
    {
      // Using a yield return statement to return each element one at a time
      yield return FromStatus;
      yield return ToStatus;
    }

    public override int GetHashCode()
    {
      return (int)FromStatus * 10 + (int)ToStatus;
    }
  }
}
