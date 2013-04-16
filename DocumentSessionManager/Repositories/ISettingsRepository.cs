using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public interface ISettingsRepository
  {
    void LoadSettings(DsmSettings settings);
    void SaveSettings(DsmSettings settings);
  }
}
