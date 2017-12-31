using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Abstract
{
  public abstract class BaseEntity
  {
    private int _id;

    public virtual int Id
    {
      get { return _id; }
      protected set { _id = value; }
    }
  }
}
