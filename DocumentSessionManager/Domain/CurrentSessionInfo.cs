using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class CurrentSessionInfo
  {
    public CurrentSessionInfo(string sessionName)
    {
      if (sessionName == null)
        throw new ArgumentNullException("sessionName");

      CurrentSessionName = sessionName;
    }

    public string CurrentSessionName { get; private set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      else if (object.ReferenceEquals(obj, this))
        return true;
      else
      {
        CurrentSessionInfo objCast = obj as CurrentSessionInfo;
        if (objCast == null)
          return false;

        if (this.CurrentSessionName.Equals(objCast.CurrentSessionName, StringComparison.OrdinalIgnoreCase))
          return true;
      }

      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return CurrentSessionName.GetHashCode();
    }
  }
}
