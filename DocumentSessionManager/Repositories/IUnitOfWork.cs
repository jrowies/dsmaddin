using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public interface IUnitOfWork
  {
    void RegisterSavedOrUpdated(object obj);
    void RegisterRemoved(object obj);
    void Commit();
    void Rollback();
  }
}
