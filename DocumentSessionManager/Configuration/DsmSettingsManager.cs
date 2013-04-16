using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class DsmSettingsManager : IDsmSettingsManager
  {
    private ISettingsRepository _settingsRepository;

    private DsmSettings _dsmSettings = new DsmSettings();

    public DsmSettings DsmSettings
    {
      get
      {
        return _dsmSettings;
      }
    }

    public DsmSettingsManager(ISettingsRepository settingsRepository)
    {
      _settingsRepository = settingsRepository;
      LoadSettings();
    }

    public void SaveSettings()
    {
      _settingsRepository.SaveSettings(_dsmSettings);
    }

    public void LoadSettings()
    {
      _settingsRepository.LoadSettings(_dsmSettings);
    }
  }
}
