using System;

namespace DocumentSessionManager
{
  public interface IDteWindowAdapter
  {
    string FullPath { get; }
    DocumentType DocumentType { get; }
    bool DocumentMatches(SessionDocument document);
    bool Close(SaveChanges saveChanges);
  }

  public enum SaveChanges
  {
    No,
    Yes,
    Prompt
  }
}
