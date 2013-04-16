using System;

namespace DocumentSessionManager
{
  public interface IExceptionManager
  {
    void HandleException(Exception ex);
  }
}
