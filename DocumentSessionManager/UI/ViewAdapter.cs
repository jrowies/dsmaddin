using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;

namespace DocumentSessionManager
{
  public class ViewAdapter : IViewAdapter
  {
    private IDteAdapter _dteAdapter;
    private IDsmSettingsManager _settings;
    private IExceptionManager _exceptionManager;

    public ViewAdapter(IDteAdapter dteAdapter, IDsmSettingsManager settings, IExceptionManager exceptionManager)
    {
      _settings = settings;
      _dteAdapter = dteAdapter;
      _exceptionManager = exceptionManager;
    }

    private FrmSessions CreateForm()
    {
      return new FrmSessions(_dteAdapter, _settings, _exceptionManager);
    }

    #region IViewAdapter Members

    public void ShowRecentlyClosedDocuments(IEnumerable<SessionDocumentDto> recentlyClosedDocuments)
    {
      using (FrmSessions frm = CreateForm())
      {
        try
        {
          frm.ConfigureForRecentlyClosedDocuments(recentlyClosedDocuments);
          frm.ShowDialog();
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
        }
      }
    }

    public SessionDto GetSessionForLoading(IEnumerable<SessionDto> currentSessions)
    {
      using (FrmSessions frm = CreateForm())
      {
        try
        {
          frm.ConfigureForLoading(currentSessions);
          frm.ShowDialog();

          if (frm.Result.Count > 0)
            return frm.Result[0];
          else
            return null;
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
          return null;
        }
      }
    }

    public IList<SessionDto> GetSessionsForDelete(IEnumerable<SessionDto> currentSessions)
    {
      using (FrmSessions frm = CreateForm())
      {
        try
        {
          frm.ConfigureForDelete(currentSessions);
          frm.ShowDialog();

          return frm.Result;
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
          return null;
        }
      }
    }

    public SessionDto GetSessionForSaveAs(IEnumerable<SessionDto> currentSessions)
    {
      using (FrmSessions frm = CreateForm())
      {
        try
        {
          frm.ConfigureForSaveAs(currentSessions);

          frm.ShowDialog();

          if (frm.Result.Count > 0)
            return frm.Result[0];
          else
            return null;
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
          return null;
        }
      }
    }

    public bool AskForConfirmation(string confirmationMsg, params object[] args)
    {
      return FrmSessions.AskForConfirmation(confirmationMsg, args);
    }

    public void ShowLongMessage(string caption, string message, string longMessage)
    {
      var frm = new FrmLongMessage(caption, message, longMessage);
      frm.ShowDialog();
    }

    #endregion
  }
}
