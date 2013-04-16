using System;
using System.Collections.Generic;

namespace DocumentSessionManager
{
  public interface IDteAdapter
  {
    IEnumerable<IDteWindowAdapter> GetWindowsForValidDocuments();
    void OpenFile(string fullPath, DocumentType type);
    bool FileExists(string path);
  }
}
