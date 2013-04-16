using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public class Session
  {
    public Session(string name)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      _isDirty = true;
      _name = name;
      _documents = new Collection<SessionDocument>();
    }

    private bool _isDirty;
    public bool IsDirty
    {
      get
      {
        return _isDirty;
      }
      set
      {
        _isDirty = value;
      }
    }

    private string _name;
    public string Name
    {
      get
      {
        return _name;
      }
    }

    private ICollection<SessionDocument> _documents;

    public void RemoveAllDocuments()
    {
      if (_documents.Count > 0)
      {
        _documents.Clear();
        IsDirty = true;
      }
    }

    public void AddDocument(SessionDocument document)
    {
      if (document == null)
        throw new ArgumentNullException("document");

      var doc = _documents.FirstOrDefault<SessionDocument>((d => d.Equals(document)));
      if (doc == null)
      {
        _documents.Add(document);
        _isDirty = true;
      }
    }

    public void RemoveDocument(SessionDocument document)
    {
      if (document == null)
        throw new ArgumentNullException("document");

      var doc = _documents.FirstOrDefault<SessionDocument>((d => d.Equals(document)));
      if (doc != null)
      {
        _documents.Remove(doc);
        _isDirty = true;
      }
    }

    public IEnumerable<SessionDocument> GetDocuments()
    {
      return _documents.AsEnumerable();
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      else if (object.ReferenceEquals(obj, this))
        return true;
      else
      {
        Session objCast = obj as Session;
        if (objCast == null)
          return false;

        if (Name.Equals(objCast.Name, StringComparison.OrdinalIgnoreCase))
          return true;
      }

      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }
  }
}
