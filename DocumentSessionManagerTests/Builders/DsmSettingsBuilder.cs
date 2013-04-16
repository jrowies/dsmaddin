using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentSessionManager;

namespace DocumentSessionManagerTests.Builders
{
  public class DsmSettingsBuilder
  {
    private DsmSettings _dsmSettings;

    public DsmSettingsBuilder StartDefault()
    {
      _dsmSettings = new DsmSettings();
      _dsmSettings.AskConfirmationOnDelete = false;
      _dsmSettings.AskConfirmationOnLoad = false;
      _dsmSettings.AskConfirmationOnReload = false;
      _dsmSettings.AskConfirmationOnSave = false;
      _dsmSettings.AskConfirmationOnSaveAs = false;

      return this;
    }

    public DsmSettings Build()
    {
      return _dsmSettings;
    }
  }
}
