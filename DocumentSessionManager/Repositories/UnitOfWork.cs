using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public class UnitOfWork : IUnitOfWork
  {
    private IRepository _repository;

    private IList<object> _savedOrUpdated = new List<object>();
    private IList<object> _removed = new List<object>();

    public UnitOfWork(IRepository repository)
    {
      _repository = repository;
    }

    public void RegisterSavedOrUpdated(object obj)
    {
      if (_removed.Contains(obj))
        throw new Exception("object already registered as removed");

      if (!_savedOrUpdated.Contains(obj))
        _savedOrUpdated.Add(obj);
      else
        throw new Exception("object already registered as saved or updated");
    }

    public void RegisterRemoved(object obj)
    {
      if (_savedOrUpdated.Contains(obj))
        throw new Exception("object already registered as saved or updated");

      if (!_removed.Contains(obj))
        _removed.Add(obj);
      else
        throw new Exception("object already registered as removed");
    }

    public void Commit()
    {
      _repository.Persist(_savedOrUpdated, _removed);
      _savedOrUpdated.Clear();
      _removed.Clear();
    }

    public void Rollback()
    {
      _savedOrUpdated.Clear();
      _removed.Clear();
    }
  }
}
