using System;
using System.Collections.Generic;

namespace DocumentSessionManager
{
  public interface IViewAdapter
  {
    void ShowRecentlyClosedDocuments(IEnumerable<SessionDocumentDto> recentlyClosedDocuments);

    SessionDto GetSessionForLoading(IEnumerable<SessionDto> currentSessions);

    IList<SessionDto> GetSessionsForDelete(IEnumerable<SessionDto> currentSessions);

    SessionDto GetSessionForSaveAs(IEnumerable<SessionDto> currentSessions);

    bool AskForConfirmation(string confirmationMsg, params object[] args);

    void ShowLongMessage(string caption, string message, string longMessage);
  }
}
