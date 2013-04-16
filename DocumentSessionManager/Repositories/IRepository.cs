using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public interface IRepository
  {
    void Persist(IList<object> savedOrUpdated, IList<object> removed);
    IUnitOfWork GetUnitOfWork();
  }
}
