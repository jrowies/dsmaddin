using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentSessionManager;

namespace DocumentSessionManagerTests.Builders
{
  public class SessionBuilder
  {
    private Session _session;

    public SessionBuilder StartDefault(string sessionName)
    {
      _session = new Session(sessionName);
      return this;
    }

    public SessionBuilder StartDefault()
    {
      _session = new Session("DEFAULT_SESSION");
      return this;
    }

    public Session Build()
    {
      return _session;
    }
  }
}
