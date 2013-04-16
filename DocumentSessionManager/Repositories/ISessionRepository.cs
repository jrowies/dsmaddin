using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public interface ISessionRepository : IRepository
  {
    IEnumerable<Session> GetAllSessions();
    string GetCurrentSessionName();
  }
}