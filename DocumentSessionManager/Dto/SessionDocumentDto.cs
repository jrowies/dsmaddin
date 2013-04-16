using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class SessionDocumentDto
  {
    private string _path;
    public string Path
    {
      get
      {
        return _path;
      }
      set
      {
        _path = value;
        _document = System.IO.Path.GetFileName(Path);
      }
    }

    public DocumentType Type { get; set; }

    private string _document;
    public string Document
    {
      get
      {
        if (Type == DocumentType.Designer)
          return String.Format("{0} [Designer]", _document);
        else
          return _document;
      }
    }
  }
}
