using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class MemorySettingsRepository : ISettingsRepository
  {
    public MemorySettingsRepository()
    {
      _inMemorySettings = new DsmSettings();
      _inMemorySettings.AskConfirmationOnDelete = true;
      _inMemorySettings.AskConfirmationOnLoad = true;
      _inMemorySettings.AskConfirmationOnReload = true;
      _inMemorySettings.AskConfirmationOnSave = true;
      _inMemorySettings.AskConfirmationOnSaveAs = true;
      _inMemorySettings.AskConfirmationRestoreDocs = true;
      _inMemorySettings.RestoreOpenedDocumentsAfterDebug = true;
    }

    private DsmSettings _inMemorySettings;
    
    public void LoadSettings(DsmSettings settings)
    {
      settings.AskConfirmationOnDelete = _inMemorySettings.AskConfirmationOnDelete;
      settings.AskConfirmationOnLoad = _inMemorySettings.AskConfirmationOnLoad;
      settings.AskConfirmationOnReload = _inMemorySettings.AskConfirmationOnReload;
      settings.AskConfirmationOnSave = _inMemorySettings.AskConfirmationOnSave;
      settings.AskConfirmationOnSaveAs = _inMemorySettings.AskConfirmationOnSaveAs;
      settings.AskConfirmationRestoreDocs = _inMemorySettings.AskConfirmationRestoreDocs;
      settings.RestoreOpenedDocumentsAfterDebug = _inMemorySettings.RestoreOpenedDocumentsAfterDebug;
    }

    public void SaveSettings(DsmSettings settings)
    {
      _inMemorySettings.AskConfirmationOnDelete = settings.AskConfirmationOnDelete;
      _inMemorySettings.AskConfirmationOnLoad = settings.AskConfirmationOnLoad;
      _inMemorySettings.AskConfirmationOnReload = settings.AskConfirmationOnReload;
      _inMemorySettings.AskConfirmationOnSave = settings.AskConfirmationOnSave;
      _inMemorySettings.AskConfirmationOnSaveAs = settings.AskConfirmationOnSaveAs;
      _inMemorySettings.AskConfirmationRestoreDocs = settings.AskConfirmationRestoreDocs;
      _inMemorySettings.RestoreOpenedDocumentsAfterDebug = settings.RestoreOpenedDocumentsAfterDebug;
    }
  }
}
