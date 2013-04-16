using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public class SessionDocument
  {
    private SessionDocument()
    {
    }

    public SessionDocument(string path, DocumentType type)
    {
      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");

      this.Path = path;
      this.Type = type;
    }

    public string Path { get; private set; }
    public DocumentType Type { get; private set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      else if (object.ReferenceEquals(obj, this))
        return true;
      else
      {
        SessionDocument objCast = obj as SessionDocument;
        if (objCast == null)
          return false;

        if ((this.Path.Equals(objCast.Path, StringComparison.OrdinalIgnoreCase)) && (this.Type == objCast.Type))
          return true;
      }

      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return (Path + Type.ToString()).GetHashCode();
    }
  }

  public enum DocumentType
  {
    Text,
    Designer
  }
}
