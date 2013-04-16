using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DocumentSessionManager
{
  public class AddinController
  {
    private ISessionManager _sessionManager;

    private IDteAdapter _dteAdapter;

    private string _selectedSessionName = null;

    private IExceptionManager _exceptionManager;

    public const int INT_RecentlyClosedDocsCapacity = 20;

    private List<SessionDocument> _recentlyClosedDocs = new List<SessionDocument>();

    private static IDsmSettingsManager _settingsManager;

    public static IDsmSettingsManager SettingsManager
    {
      get
      {
        return _settingsManager;
      }
    }

    private IViewAdapter _viewAdapter;

    public AddinController(ISessionManager sessionManager, IDsmSettingsManager settingsManager, IViewAdapter viewAdapter, IDteAdapter dteAdapter, 
      IExceptionManager exceptionManager)
    {
      _exceptionManager = exceptionManager;
      _viewAdapter = viewAdapter;
      _settingsManager = settingsManager;
      _dteAdapter = dteAdapter;
      _sessionManager = sessionManager;

      if (_sessionManager.CurrentSession != null)
        _selectedSessionName = _sessionManager.CurrentSession.Name;
    }

    public static void NotifyPluginUnloaded()
    {
      _settingsManager = null;
    }

    public string SelectedSessionName
    {
      get
      {
        return _selectedSessionName;
      }
      set
      {
        _selectedSessionName = value;
      }
    }

    public void RefreshCurrentSession()
    {
      if (_sessionManager.SetCurrentSessionByName(_selectedSessionName))
        _sessionManager.Persist();
    }

    public IEnumerable<string> GetSessionNames()
    {
      return _sessionManager.GetSessionNames();
    }

    public void ShowRecentlyClosedDocuments()
    {
      var dtoList = new List<SessionDocumentDto>();
      foreach (var obj in _recentlyClosedDocs)
      {
        var dto = new SessionDocumentDto();
        DtoMapper.MapObjToDto(obj, dto);
        dtoList.Add(dto);
      }

      _viewAdapter.ShowRecentlyClosedDocuments(dtoList);
    }

    private static List<SessionDto> getSessionDtoList(IEnumerable<Session> sessionList)
    {
      var dtoList = new List<SessionDto>();

      foreach (var obj in sessionList)
      {
        var dto = new SessionDto();
        DtoMapper.MapObjToDto(obj, dto);
        dtoList.Add(dto);
      }

      return dtoList;
    }

    public void LoadSession()
    {
      IList<SessionDto> dtoList = getSessionDtoList(_sessionManager.GetSessions());
      SessionDto sessionDto = _viewAdapter.GetSessionForLoading(dtoList);

      if (sessionDto != null)
      {
        Session session = _sessionManager.GetSession(sessionDto.Name);
        _sessionManager.CurrentSession = session;
        _selectedSessionName = session.Name;
        LoadDocumentsFromSession(session);
      }
    }

    public void DeleteSessions()
    {
      IList<SessionDto> dtoList = getSessionDtoList(_sessionManager.GetSessions());
      IList<SessionDto> sessionsToDelete = _viewAdapter.GetSessionsForDelete(dtoList);

      if (sessionsToDelete.Count > 0)
      {
        foreach (SessionDto s in sessionsToDelete)
        {
          Session session = _sessionManager.GetSession(s.Name);

          if ((_sessionManager.CurrentSession != null) && (_sessionManager.CurrentSession.Equals(session)))
            _selectedSessionName = string.Empty;

          _sessionManager.RemoveSession(session);
        }

        _sessionManager.Persist();
      }
    }

    public void ReloadSession()
    {
      Session session = _sessionManager.CurrentSession;
      if (session != null)
      {
        if (!_settingsManager.DsmSettings.AskConfirmationOnReload || _viewAdapter.AskForConfirmation("Session '{0}' will be reloaded", session.Name))
          LoadDocumentsFromSession(session);
      }
    }

    public void SaveSession()
    {
      if (_sessionManager.CurrentSession == null)
        SaveSessionAs();
      else
      {
        if (!_settingsManager.DsmSettings.AskConfirmationOnSave || _viewAdapter.AskForConfirmation("Session '{0}' will be replaced", _sessionManager.CurrentSession.Name))
        {
          FillDocumentsInSession(_sessionManager.CurrentSession);
          _sessionManager.Persist();
        }
      }
    }

    public void SaveSessionAs()
    {
      IList<SessionDto> dtoList = getSessionDtoList(_sessionManager.GetSessions());
      SessionDto newSessionDto = _viewAdapter.GetSessionForSaveAs(dtoList);

      if (newSessionDto != null)
      {
        Session newSession;

        Session sessionToReplace = _sessionManager.GetSession(newSessionDto.Name);
        if (sessionToReplace != null)
        {
          newSession = sessionToReplace;
          newSession.RemoveAllDocuments();
        }
        else
        {
          newSession = new Session(newSessionDto.Name);
          _sessionManager.AddSession(newSession);
        }
        
        _sessionManager.CurrentSession = newSession;
        _selectedSessionName = newSession.Name;

        FillDocumentsInSession(newSession);

        _sessionManager.Persist();
      }
    }

    public void FillDocumentsInSession(Session s)
    {
      var currentDocuments = new List<SessionDocument>();

      foreach (var window in _dteAdapter.GetWindowsForValidDocuments())
      {
        var doc = new SessionDocument(window.FullPath, window.DocumentType);
        currentDocuments.Add(doc);
      }

      var sessionDocuments = new List<SessionDocument>(s.GetDocuments());
      for (int i = sessionDocuments.Count - 1; i >= 0; i--)
      {
        if (!currentDocuments.Contains(sessionDocuments[i]))
          s.RemoveDocument(sessionDocuments[i]);
      }

      foreach (var currDoc in currentDocuments)
        s.AddDocument(currDoc);
    }

    public void AddDocumentsToRecentlyClosedList(List<SessionDocument> elementsToAdd)
    {
      AddDocumentsToRecentlyClosedList(_recentlyClosedDocs, elementsToAdd);
    }

    public static void AddDocumentsToRecentlyClosedList(List<SessionDocument> recentlyClosedDocs, List<SessionDocument> elementsToAdd)
    {
      foreach (var d in elementsToAdd)
        recentlyClosedDocs.Remove(d);

      int qtyElementsToDelete = Math.Max((recentlyClosedDocs.Count + elementsToAdd.Count) - INT_RecentlyClosedDocsCapacity, 0);

      if (qtyElementsToDelete > recentlyClosedDocs.Count)
        recentlyClosedDocs.Clear();
      else
        recentlyClosedDocs.RemoveRange(recentlyClosedDocs.Count - qtyElementsToDelete, qtyElementsToDelete);

      foreach (var d in elementsToAdd)
        recentlyClosedDocs.Insert(0, d);
    }

    public void LoadDocumentsFromSession(Session session)
    {
      try
      {
        bool cancelled = false;

        var localClosedDocs = new List<SessionDocument>();

        var documents = new List<SessionDocument>(session.GetDocuments());

        foreach (var window in _dteAdapter.GetWindowsForValidDocuments())
        {
          int index = -1;
          for (int i = 0; i < documents.Count; i++)
          {
            if (window.DocumentMatches(documents[i]))
            {
              index = i;
              break;
            }
          }

          if (index < 0)
          {
            string fullPath = window.FullPath;
            DocumentType documentType = window.DocumentType;

            if (!window.Close(SaveChanges.Prompt))
            {
              cancelled = true;
              break;
            }
            else
              localClosedDocs.Insert(0, new SessionDocument(fullPath, documentType));

          }
          else
            documents.RemoveAt(index);
        }

        if (localClosedDocs.Count > 0)
        {
          AddDocumentsToRecentlyClosedList(_recentlyClosedDocs, localClosedDocs);
        }

        if (!cancelled)
        {
          StringBuilder errors = new StringBuilder();
          foreach (SessionDocument document in documents)
          {
            if (!_dteAdapter.FileExists(document.Path))
              errors.AppendLine(document.Path);
            else
              _dteAdapter.OpenFile(document.Path, document.Type);
          }

          if (errors.Length > 0)
          {
            _viewAdapter.ShowLongMessage("Warning", "The following documents could not be found", errors.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        _exceptionManager.HandleException(ex);
      }
    }

    public bool DeleteSessionsEnabled()
    {
      return _sessionManager.SessionCount > 0;
    }

    public bool LoadSessionEnabled()
    {
      return _sessionManager.SessionCount > 0;
    }

    public bool RecentlyClosedEnabled()
    {
      return _recentlyClosedDocs.Count > 0;
    }

    public bool ReloadSessionEnabled()
    {
      return _sessionManager.CurrentSession != null;
    }

    public bool SaveSessionEnabled()
    {
      return _sessionManager.CurrentSession != null;
    }

    public bool SaveSessionAsEnabled()
    {
      return true;
    }
  }
}
