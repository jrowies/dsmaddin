using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;

namespace DocumentSessionManager
{
  public class Container
  {
    private DebuggerHook _debugHook;
    public DebuggerHook GetDebugHook(DTE2 dteApplicationObject)
    {
      if (_debugHook == null)
        _debugHook = new DebuggerHook(dteApplicationObject, GetAddinController(dteApplicationObject), GetIViewAdapter(dteApplicationObject), GetIDsmSettingsManager());

      return _debugHook;
    }

    private AddinController _addinController;
    public AddinController GetAddinController(DTE2 dteApplicationObject)
    {
      if (_addinController == null)
        _addinController = new AddinController(GetISessionManager(), GetIDsmSettingsManager(), GetIViewAdapter(dteApplicationObject), 
          GetIDteAdapter(dteApplicationObject), GetIExceptionManager());

      return _addinController;
    }

    private ISessionManager _sessionManager;
    public ISessionManager GetISessionManager()
    {
      if (_sessionManager == null)
        _sessionManager = new SessionManager(GetISessionRepository());

      return _sessionManager;
    }

    private IDteAdapter _dteAdapter;
    public IDteAdapter GetIDteAdapter(DTE2 dteApplicationObject)
    {
      if (_dteAdapter == null) 
        _dteAdapter = new DteAdapter(dteApplicationObject);

      return _dteAdapter;
    }

    private IViewAdapter _viewAdapter;
    public IViewAdapter GetIViewAdapter(DTE2 dteApplicationObject)
    {
      if (_viewAdapter == null)
        _viewAdapter = new ViewAdapter(GetIDteAdapter(dteApplicationObject), GetIDsmSettingsManager(), GetIExceptionManager());

      return _viewAdapter;
    }

    private IExceptionManager _exceptionManager;
    public IExceptionManager GetIExceptionManager()
    {
      if (_exceptionManager == null)
        _exceptionManager = new ExceptionManager();

      return _exceptionManager;
    }

    private IDsmSettingsManager _dsmSettingsManager;
    public IDsmSettingsManager GetIDsmSettingsManager()
    {
      if (_dsmSettingsManager == null)
        _dsmSettingsManager = new DsmSettingsManager(GetISettingsRepository());

      return _dsmSettingsManager;
    }

    private ISettingsRepository _settingsRepository;
    public ISettingsRepository GetISettingsRepository()
    {
      //if (_settingsRepository == null)
      //  _settingsRepository = new MemorySettingsRepository();

      if (_settingsRepository == null)
        _settingsRepository = new XmlSettingsRepository();

      return _settingsRepository;
    }

    private ISessionRepository _sessionRepository;
    public ISessionRepository GetISessionRepository()
    {
      //if (_sessionRepository == null)
      //  _sessionRepository = new MemorySessionRepository();

      if (_sessionRepository == null)
        _sessionRepository = new XmlSessionRepository();

      return _sessionRepository;
    }
  }
}
