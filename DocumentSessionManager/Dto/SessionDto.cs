using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class SessionDto
  {
    public string Name { get; set; }

    private IList<SessionDocumentDto> _documents = new List<SessionDocumentDto>();
    public IList<SessionDocumentDto> Documents
    {
      get
      {
        return _documents;
      }
    }

    public int DocumentsCount
    {
      get
      {
        return _documents.Count;
      }
    }

  }
}
