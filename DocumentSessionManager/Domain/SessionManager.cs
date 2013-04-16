using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public class SessionManager : ISessionManager
  {
    private ISessionRepository _repository;

    public SessionManager(ISessionRepository repository)
    {
      _repository = repository;
      _sessionsCache = new Collection<Session>();
      _removedSessionsCache = new Collection<Session>();

      Load();
    }

    private Session _currentSession;
    public Session CurrentSession
    {
      get
      {
        return _currentSession;
      }
      set
      {
        if (_currentSession != value)
          _currentSession = value;
      }
    }

    public int SessionCount 
    { 
      get 
      {
        return _sessionsCache.Count;
      } 
    }

    private ICollection<Session> _sessionsCache;

    private ICollection<Session> _removedSessionsCache;

    public Session GetSession(string name)
    {
      return _sessionsCache.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public void AddSession(Session session)
    {
      if (session == null)
        throw new ArgumentNullException("session");

      if (GetSession(session.Name) != null)
        throw new Exception(string.Format("Trying to add duplicated session: {0}", session.Name));

      session.IsDirty = true;
      _sessionsCache.Add(session);
    }

    public void RemoveSession(Session session)
    {
      if (session == null)
        throw new ArgumentNullException("session");

      if (GetSession(session.Name) == null)
        throw new Exception(string.Format("Trying to remove non existent session: {0}", session.Name));

      _removedSessionsCache.Add(session);
      _sessionsCache.Remove(session);

      if ((_currentSession != null) && (_currentSession.Equals(session)))
        _currentSession = null;
    }

    public IEnumerable<Session> GetSessions()
    {
      return _sessionsCache.AsEnumerable();
    }

    public void Load()
    {
      var loadedSessions = _repository.GetAllSessions();

      _removedSessionsCache.Clear();
      _sessionsCache.Clear();

      foreach (var s in loadedSessions)
      {
        s.IsDirty = false;
        _sessionsCache.Add(s);
      }

      string name = _repository.GetCurrentSessionName();
      CurrentSession = _sessionsCache.FirstOrDefault<Session>((s => s.Name.Equals(name, StringComparison.InvariantCulture)));
    }

    public void Persist()
    {
      IUnitOfWork uow = _repository.GetUnitOfWork();

      try
      {
        foreach (var session in _sessionsCache)
        {
          if (session.IsDirty)
            uow.RegisterSavedOrUpdated(session);
        }

        foreach (var s in _removedSessionsCache)
          uow.RegisterRemoved(s);

        string currSessionName = CurrentSession == null ? string.Empty : CurrentSession.Name;
        uow.RegisterSavedOrUpdated(new CurrentSessionInfo(currSessionName));

        uow.Commit();

        foreach (var session in _sessionsCache)
          session.IsDirty = false;
        _removedSessionsCache.Clear();
      }
      catch 
      {
        uow.Rollback();
        throw;
      }

    }

    public bool SetCurrentSessionByName(string sessionName)
    {
      if (((CurrentSession == null) && !string.IsNullOrEmpty(sessionName)) ||
        ((CurrentSession != null) && !CurrentSession.Name.Equals(sessionName)))
      {
        if (!string.IsNullOrEmpty(sessionName))
          CurrentSession = GetSession(sessionName);
        else
          CurrentSession = null;

        return true;
      }

      return false;
    }

    public IEnumerable<string> GetSessionNames()
    {
      var list = new List<string>();
      foreach (var s in GetSessions())
        list.Add(s.Name);
      return list;
    }

  }
}
