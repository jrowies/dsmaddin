using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Extensibility;
using System.Diagnostics;
using EnvDTE80;

namespace DocumentSessionManager
{
  public class DebuggerHook
  {
    public DebuggerHook(DTE2 applicationObject, AddinController controller, IViewAdapter viewAdapter, IDsmSettingsManager settingsManager)
    {
      _settingsManager = settingsManager;
      _viewAdapter = viewAdapter;
      _controller = controller;
      _applicationObject = applicationObject;
    }

    public void Hook()
    {
      _applicationObject.Events.DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
      _applicationObject.Events.DebuggerEvents.OnEnterDesignMode += DebuggerEvents_OnEnterDesignMode;
    }

    public void UnHook()
    {
      _applicationObject.Events.DebuggerEvents.OnEnterRunMode -= DebuggerEvents_OnEnterRunMode;
      _applicationObject.Events.DebuggerEvents.OnEnterDesignMode -= DebuggerEvents_OnEnterDesignMode;
    }

    private bool _debuggerRunning;
    private AddinController _controller;
    private Session _preDebugSession;
    private IViewAdapter _viewAdapter;
    private IDsmSettingsManager _settingsManager;
    private DTE2 _applicationObject;

    private static bool reasonIsStopDebugger(dbgEventReason Reason)
    {
      return (Reason == dbgEventReason.dbgEventReasonEndProgram) ||
        (Reason == dbgEventReason.dbgEventReasonStopDebugging) ||
        (Reason == dbgEventReason.dbgEventReasonDetachProgram);
    }

    private static bool reasonIsStartDebugger(dbgEventReason Reason)
    {
      return (Reason == dbgEventReason.dbgEventReasonGo) ||
        (Reason == dbgEventReason.dbgEventReasonAttachProgram);
    }

    private void DebuggerEvents_OnEnterRunMode(dbgEventReason Reason)
    {
      if (!_debuggerRunning && reasonIsStartDebugger(Reason))
      {
        _debuggerRunning = true;

        _preDebugSession = new Session("preDebugSession");
        _controller.FillDocumentsInSession(_preDebugSession);
      }
    }

    private void DebuggerEvents_OnEnterDesignMode(dbgEventReason Reason)
    {
      if (_debuggerRunning && reasonIsStopDebugger(Reason))
      {
        _debuggerRunning = false;

        try
        {
          if (_settingsManager.DsmSettings.RestoreOpenedDocumentsAfterDebug)
          {
            var postDebugSession = new Session("postDebugSession");
            _controller.FillDocumentsInSession(postDebugSession);

            bool documentsChanged = false;

            if (_preDebugSession.GetDocuments().Count() != postDebugSession.GetDocuments().Count())
              documentsChanged = true;
            else
            {
              var postDebugSessionDocs = postDebugSession.GetDocuments();
              foreach (var doc in _preDebugSession.GetDocuments())
              {
                if (!postDebugSessionDocs.Contains(doc))
                {
                  documentsChanged = true;
                  break;
                }
              }
            }

            if (documentsChanged)
            {
              if (!_settingsManager.DsmSettings.AskConfirmationRestoreDocs ||
                _viewAdapter.AskForConfirmation("Do you want to restore the documents as they were before start debugging ?\r\n" +
                "(Please note that closed documents can be re-opened later through the 'Recently Closed Documents' addin button)"))
              {
                _controller.LoadDocumentsFromSession(_preDebugSession);
              }
            }
          }
        }
        finally
        {
          _preDebugSession = null;
        }

      }
    }
  }
}
