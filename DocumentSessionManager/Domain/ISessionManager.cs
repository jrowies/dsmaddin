using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public interface ISessionManager
  {
    Session CurrentSession { get; set; }
    int SessionCount { get; }
    Session GetSession(string name);
    void AddSession(Session session);
    void RemoveSession(Session session);
    IEnumerable<Session> GetSessions();
    void Load();
    void Persist();
    bool SetCurrentSessionByName(string sessionName);
    IEnumerable<string> GetSessionNames();
  }
}
