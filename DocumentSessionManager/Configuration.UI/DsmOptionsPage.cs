using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DocumentSessionManager
{
  public partial class DsmOptionsPage : UserControl, EnvDTE.IDTToolsOptionsPage
  {
    public DsmOptionsPage()
    {
      InitializeComponent();
    }

    #region IDTToolsOptionsPage Members

    public void GetProperties(ref object PropertiesObject)
    {
      PropertiesObject = null;
    }

    public void OnAfterCreated(EnvDTE.DTE DTEObject)
    {
      bool optionsEnabled = (AddinController.SettingsManager != null);

      label1.Visible = !optionsEnabled;

      checkAskConfirmationOnDelete.Visible = optionsEnabled;
      checkAskConfirmationOnLoad.Visible = optionsEnabled;
      checkAskConfirmationOnReload.Visible = optionsEnabled;
      checkAskConfirmationOnSave.Visible = optionsEnabled;
      checkAskConfirmationOnSaveAs.Visible = optionsEnabled;
      checkAskConfirmationRestoreDocs.Visible = optionsEnabled;
      checkRestoreOpenedDocumentsAfterDebug.Visible = optionsEnabled;

      if (optionsEnabled)
      {
        checkAskConfirmationOnDelete.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnDelete;
        checkAskConfirmationOnLoad.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnLoad;
        checkAskConfirmationOnReload.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnReload;
        checkAskConfirmationOnSave.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnSave;
        checkAskConfirmationOnSaveAs.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnSaveAs;
        checkAskConfirmationRestoreDocs.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationRestoreDocs;
        checkRestoreOpenedDocumentsAfterDebug.Checked = AddinController.SettingsManager.DsmSettings.RestoreOpenedDocumentsAfterDebug;
      }
    }

    public void OnCancel()
    {
      if (AddinController.SettingsManager != null)
      {
        checkAskConfirmationOnDelete.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnDelete;
        checkAskConfirmationOnLoad.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnLoad;
        checkAskConfirmationOnReload.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnReload;
        checkAskConfirmationOnSave.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnSave;
        checkAskConfirmationOnSaveAs.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationOnSaveAs;
        checkAskConfirmationRestoreDocs.Checked = AddinController.SettingsManager.DsmSettings.AskConfirmationRestoreDocs;
        checkRestoreOpenedDocumentsAfterDebug.Checked = AddinController.SettingsManager.DsmSettings.RestoreOpenedDocumentsAfterDebug;
      }
    }

    public void OnHelp()
    {
    }

    public void OnOK()
    {
      if (AddinController.SettingsManager != null)
      {
        AddinController.SettingsManager.DsmSettings.AskConfirmationOnDelete = checkAskConfirmationOnDelete.Checked;
        AddinController.SettingsManager.DsmSettings.AskConfirmationOnLoad = checkAskConfirmationOnLoad.Checked;
        AddinController.SettingsManager.DsmSettings.AskConfirmationOnReload = checkAskConfirmationOnReload.Checked;
        AddinController.SettingsManager.DsmSettings.AskConfirmationOnSave = checkAskConfirmationOnSave.Checked;
        AddinController.SettingsManager.DsmSettings.AskConfirmationOnSaveAs = checkAskConfirmationOnSaveAs.Checked;
        AddinController.SettingsManager.DsmSettings.AskConfirmationRestoreDocs = checkAskConfirmationRestoreDocs.Checked;
        AddinController.SettingsManager.DsmSettings.RestoreOpenedDocumentsAfterDebug = checkRestoreOpenedDocumentsAfterDebug.Checked;

        AddinController.SettingsManager.SaveSettings();
      }
    }

    #endregion

    private void checkRestoreOpenedDocumentsAfterDebug_CheckedChanged(object sender, EventArgs e)
    {
      checkAskConfirmationRestoreDocs.Enabled = checkRestoreOpenedDocumentsAfterDebug.Checked;
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Process.Start(linkLabel1.Text);
      }
      catch 
      {
      }
    }
  }
}
