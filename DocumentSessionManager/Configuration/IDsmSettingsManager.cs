using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public interface IDsmSettingsManager
  {
    DsmSettings DsmSettings { get; }

    void SaveSettings();

    void LoadSettings();
  }
}
