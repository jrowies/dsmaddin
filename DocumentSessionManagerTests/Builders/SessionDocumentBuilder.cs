using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentSessionManager;

namespace DocumentSessionManagerTests.Builders
{
  public class SessionDocumentBuilder
  {
    private SessionDocument _sessionDocument;

    public SessionDocumentBuilder StartDefault(string path, DocumentType type)
    {
      _sessionDocument = new SessionDocument(path, type);
      return this;
    }

    public SessionDocumentBuilder StartDefault(string path)
    {
      _sessionDocument = new SessionDocument(path, DocumentType.Text);
      return this;
    }

    public SessionDocumentBuilder StartDefault()
    {
      _sessionDocument = new SessionDocument("DEFAULT_PATH", DocumentType.Text);
      return this;
    }

    public SessionDocument Build()
    {
      return _sessionDocument;
    }
  }
}
