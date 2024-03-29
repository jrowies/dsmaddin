using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Collections.ObjectModel;
using DocumentSessionManager.Properties;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using stdole;
using System.Diagnostics;

namespace DocumentSessionManager
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
  public class Connect : IDTExtensibility2, IDTCommandTarget
  {
    private const string STR_DocumentSessionManager = "DocumentSessionManager";

    private const string COMMAND_DsmSaveSession = "DocumentSessionManager.Connect.DsmSaveSession";
    private const string COMMAND_DsmSaveSessionAs = "DocumentSessionManager.Connect.DsmSaveSessionAs";
    private const string COMMAND_DsmReloadSession = "DocumentSessionManager.Connect.DsmReloadSession";
    private const string COMMAND_DsmLoadSession = "DocumentSessionManager.Connect.DsmLoadSession";
    private const string COMMAND_DsmDeleteSessions = "DocumentSessionManager.Connect.DsmDeleteSessions";
    private const string COMMAND_DsmRecentlyClosed = "DocumentSessionManager.Connect.DsmRecentlyClosed";
    private const string COMMAND_DsmSessionList = "DocumentSessionManager.Connect.DsmSessionList";
    private const string COMMAND_DsmSessionList_1 = "DocumentSessionManager.Connect.DsmSessionList_1";

    private CommandBar managerToolbar;
    private DTE2 _applicationObject;
    private AddIn _addInInstance;
    //private Settings _settings;
    private List<CommandBarControl> _controlsCreated = new List<CommandBarControl>();
    private IExceptionManager _exceptionManager;
    private IDsmSettingsManager _settingsManager;
    private DebuggerHook _debugHook;

    /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
    public Connect()
    {
    }

    private AddinController _controller;

    /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
    /// <param term='application'>Root object of the host application.</param>
    /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
    /// <param term='addInInst'>Object representing this Add-in.</param>
    /// <seealso class='IDTExtensibility2' />
    public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
    {
      try
      {
        _applicationObject = (DTE2)application;

        _addInInstance = (AddIn)addInInst;

        var container = new Container();

        _exceptionManager = container.GetIExceptionManager();

        _controller = container.GetAddinController(_applicationObject);

        _debugHook = container.GetDebugHook(_applicationObject);
        _debugHook.Hook();

        _settingsManager = container.GetIDsmSettingsManager();

        if ((connectMode == ext_ConnectMode.ext_cm_Startup) || (connectMode == ext_ConnectMode.ext_cm_AfterStartup))
        {
          //_settings = Settings.Default;
          //_settings.Reload();

          CommandBars commandBars = (CommandBars)_applicationObject.CommandBars;

          // Create a toolbar for this app
          string managerToolbarName = "Document Session Manager";
          managerToolbar = null;
          //try
          //{
          //  managerToolbar = ((Microsoft.VisualStudio.CommandBars.CommandBars)this._applicationObject.CommandBars)[managerToolbarName];
          //}
          //catch (ArgumentException)
          //{
          //}

          if (managerToolbar == null)
          {
            managerToolbar = (CommandBar)commandBars.Add(managerToolbarName, MsoBarPosition.msoBarTop, System.Type.Missing, true);

            _settingsManager.LoadSettings();
            managerToolbar.Position = (MsoBarPosition)Enum.Parse(typeof(MsoBarPosition), 
              _settingsManager.DsmSettings.ToolBarPosition ?? MsoBarPosition.msoBarTop.ToString());
            managerToolbar.RowIndex = _settingsManager.DsmSettings.ToolBarRowIndex;
            managerToolbar.Visible = _settingsManager.DsmSettings.ToolBarVisible;

            if (_settingsManager.DsmSettings.ToolBarTop != -1)
              managerToolbar.Top = _settingsManager.DsmSettings.ToolBarTop;
            if (_settingsManager.DsmSettings.ToolBarLeft != -1)
              managerToolbar.Left = _settingsManager.DsmSettings.ToolBarLeft;
            //if (_settingsManager.DsmSettings.ToolBarWidth != -1)
            //  managerToolbar.Width = _settingsManager.DsmSettings.ToolBarWidth;
            //if (_settingsManager.DsmSettings.ToolBarHeight != -1)
            //  managerToolbar.Height = _settingsManager.DsmSettings.ToolBarHeight;

          }

          object controlCreated;

          _controlsCreated.Clear();

          //PictureManager.CommandButtons.Clear();

          AddCommand(managerToolbar, "DsmSessionList", "DsmSessionList",
            "Sessions", vsCommandControlType.vsCommandControlTypeDropDownCombo, 1,
            vsCommandStyle.vsCommandStyleComboCaseSensitive, 0, out controlCreated);

          CommandBarComboBox combo = controlCreated as CommandBarComboBox;
          if (combo != null)
            combo.Width = 200;

          AddCommand(managerToolbar, "DsmLoadSession", "DsmLoadSession",
            "Load session", vsCommandControlType.vsCommandControlTypeButton, 1,
            vsCommandStyle.vsCommandStylePict, 2, out controlCreated);

          AddCommand(managerToolbar, "DsmSaveSessionAs", "DsmSaveSessionAs",
            "Save session as", vsCommandControlType.vsCommandControlTypeButton, 2,
            vsCommandStyle.vsCommandStylePict, 6, out controlCreated);

          AddCommand(managerToolbar, "DsmSaveSession", "DsmSaveSession",
            "Save session", vsCommandControlType.vsCommandControlTypeButton, 3,
            vsCommandStyle.vsCommandStylePict, 5, out controlCreated);

          AddCommand(managerToolbar, "DsmDeleteSessions", "DsmDeleteSessions",
            "Delete sessions", vsCommandControlType.vsCommandControlTypeButton, 4,
            vsCommandStyle.vsCommandStylePict, 1, out controlCreated);

          AddCommand(managerToolbar, "DsmReloadSession", "DsmReloadSession",
            "Reload session", vsCommandControlType.vsCommandControlTypeButton, 5,
            vsCommandStyle.vsCommandStylePict, 4, out controlCreated);

          AddCommand(managerToolbar, "DsmRecentlyClosed", "DsmRecentlyClosed",
            "Recently closed documents", vsCommandControlType.vsCommandControlTypeButton, 6,
            vsCommandStyle.vsCommandStylePict, 3, out controlCreated);

        }
      }
      catch (Exception ex)
      {
        _exceptionManager.HandleException(ex);
      }
    }

    /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
    /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
    /// <param term='custom'>Array of parameters that are host application specific.</param>
    /// <seealso class='IDTExtensibility2' />
    public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
    {
      try
      {
        if ((disconnectMode == ext_DisconnectMode.ext_dm_HostShutdown) || (disconnectMode == ext_DisconnectMode.ext_dm_UserClosed))
        {
          if (_debugHook != null)
            _debugHook.UnHook();

          AddinController.NotifyPluginUnloaded();

          foreach (var control in _controlsCreated)
          {
            try
            {
              control.Delete(true);
            }
            catch
            {
            }
          }

          if (managerToolbar != null)
          {
            try
            {
              if (managerToolbar.Position != null)
                _settingsManager.DsmSettings.ToolBarPosition = managerToolbar.Position.ToString();
              _settingsManager.DsmSettings.ToolBarRowIndex = managerToolbar.RowIndex;
              _settingsManager.DsmSettings.ToolBarVisible = managerToolbar.Visible;
              _settingsManager.DsmSettings.ToolBarTop = managerToolbar.Top;
              _settingsManager.DsmSettings.ToolBarLeft = managerToolbar.Left;
              //_settingsManager.DsmSettings.ToolBarWidth = managerToolbar.Width;
              //_settingsManager.DsmSettings.ToolBarHeight = managerToolbar.Height;

              _settingsManager.SaveSettings();
            }
            finally
            {
              managerToolbar.Delete();
              managerToolbar = null;
            }
          }
        }
      }
      catch (Exception ex)
      {
        _exceptionManager.HandleException(ex);
      }
    }

    /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
    /// <param term='custom'>Array of parameters that are host application specific.</param>
    /// <seealso class='IDTExtensibility2' />		
    public void OnAddInsUpdate(ref Array custom)
    {
    }

    /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
    /// <param term='custom'>Array of parameters that are host application specific.</param>
    /// <seealso class='IDTExtensibility2' />
    public void OnStartupComplete(ref Array custom)
    {
    }

    /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
    /// <param term='custom'>Array of parameters that are host application specific.</param>
    /// <seealso class='IDTExtensibility2' />
    public void OnBeginShutdown(ref Array custom)
    {
    }

    /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
    /// <param term='commandName'>The name of the command to determine state for.</param>
    /// <param term='neededText'>Text that is needed for the command.</param>
    /// <param term='status'>The state of the command in the user interface.</param>
    /// <param term='commandText'>Text requested by the neededText parameter.</param>
    /// <seealso class='Exec' />
    public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
    {
      if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
      {
        bool enabled;
        if (commandName == COMMAND_DsmDeleteSessions)
          enabled = _controller.DeleteSessionsEnabled();
        else if (commandName == COMMAND_DsmLoadSession)
          enabled = _controller.LoadSessionEnabled();
        else if (commandName == COMMAND_DsmRecentlyClosed)
          enabled = _controller.RecentlyClosedEnabled();
        else if (commandName == COMMAND_DsmReloadSession)
          enabled = _controller.ReloadSessionEnabled();
        else if (commandName == COMMAND_DsmSaveSession)
          enabled = _controller.SaveSessionEnabled();
        else if (commandName == COMMAND_DsmSaveSessionAs)
          enabled = _controller.SaveSessionAsEnabled();
        else if (commandName == COMMAND_DsmSessionList)
          enabled = true;
        else if (commandName == COMMAND_DsmSessionList_1)
          enabled = true;
        else
          enabled = false;

        //if (PictureManager.SetPicture(commandName, enabled))
        //  enabled = true;

        status = (enabled ?
          (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled :
          vsCommandStatus.vsCommandStatusSupported);

        return;
      }
    }

    /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
    /// <param term='commandName'>The name of the command to execute.</param>
    /// <param term='executeOption'>Describes how the command should be run.</param>
    /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
    /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
    /// <param term='handled'>Informs the caller if the command was handled or not.</param>
    /// <seealso class='Exec' />
    public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
    {
      try
      {
        if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
        {
          if (commandName == COMMAND_DsmSaveSession)
          {
            _controller.SaveSession();
            handled = true;
          }
          else if (commandName == COMMAND_DsmSaveSessionAs)
          {
            _controller.SaveSessionAs();
            handled = true;
          }
          else if (commandName == COMMAND_DsmReloadSession)
          {
            _controller.ReloadSession();
            handled = true;
          }
          else if (commandName == COMMAND_DsmLoadSession)
          {
            _controller.LoadSession();
            handled = true;
          }
          else if (commandName == COMMAND_DsmDeleteSessions)
          {
            _controller.DeleteSessions();
            handled = true;
          }
          else if (commandName == COMMAND_DsmRecentlyClosed)
          {
            _controller.ShowRecentlyClosedDocuments();
            handled = true;
          }
          else if (commandName == COMMAND_DsmSessionList)
          {
            if ((varIn != null) && (varIn is string))
            {
              //We are being told that the user made a selection in the combo, and we need to react to it.
              _controller.SelectedSessionName = varIn as string;
              _controller.RefreshCurrentSession();
            }
            else
            {
              //The text of the selected item is being asked for, return this through the varOut param.
              varOut = (_controller.SelectedSessionName ?? string.Empty);
            }

            handled = true;
          }
          else if (commandName == COMMAND_DsmSessionList_1)
          {
            //We are being asked for the items in the combo box, return them through the varOut param.

            IEnumerable<string> sessionNames = _controller.GetSessionNames();
            if (sessionNames.Count() == 0)
              varOut = null;
            else
              varOut = sessionNames.ToArray();

            handled = true;
          }
        }
      }
      catch (Exception ex)
      {
        _exceptionManager.HandleException(ex);
      }
    }

    private Command AddCommand(CommandBar toolbar, string key, string name, string tooltip,
      vsCommandControlType ctype, int toolbarpos, vsCommandStyle cmdstyle, int faceID, out object controlCreated)
    {

      object[] contextGUIDS = new object[] { };
      Commands2 commands = (Commands2)_applicationObject.Commands;

      // Add a command to the Commands collection:
      Command command = null;
      try
      {
        command = commands.Item(String.Format("{0}.{1}", _addInInstance.ProgID, key), -1);
      }
      catch (ArgumentException)
      {
      }

      if (command == null)
      {
        command = commands.AddNamedCommand2(_addInInstance, key, name, tooltip, false, faceID, ref contextGUIDS,
          (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
          (int)cmdstyle, ctype);
      }

      if (command != null && toolbar != null && toolbarpos > 0)
      {
        controlCreated = command.AddControl(toolbar, toolbarpos);
        CommandBarControl aux = controlCreated as CommandBarControl;
        if (aux != null)
          _controlsCreated.Add(aux);

        //if (controlCreated is CommandBarButton)
        //{
        //  PictureManager.CommandButtons.Add(key, controlCreated as CommandBarButton);
        //  PictureManager.SetPicture(key, true);
        //}
      }
      else
        controlCreated = null;

      return command;
    }

  }
}