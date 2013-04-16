using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DocumentSessionManager
{
  public class XmlSettingsRepository : ISettingsRepository
  {
    private const string TAG_SETTINGS = "Settings";

    public XmlSettingsRepository()
    {
      if (!File.Exists(FilePath))
        createFile();
    }

    private static void createFile()
    {
      var repositoryFile = new XmlDocument();

      var xmlDecl = repositoryFile.CreateNode(XmlNodeType.XmlDeclaration, "", "");
      repositoryFile.AppendChild(xmlDecl);

      var xmlSettingsNode = repositoryFile.CreateElement("", TAG_SETTINGS, "");
      repositoryFile.AppendChild(xmlSettingsNode);

      repositoryFile.Save(FilePath);
    }

    private XmlDocument getRepositoryFile()
    {
      var repositoryFile = new XmlDocument();
      repositoryFile.Load(_filePath);
      return repositoryFile;
    }

    private const string FILE = "DsmSettings.xml";

    private static string _filePath;
    public static string FilePath
    {
      get
      {
        if (string.IsNullOrEmpty(_filePath))
        {
          string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Remove(0, "file:///".Length));
          _filePath = Path.Combine(currentPath, FILE);
        }

        return _filePath;
      }
    }

    public void LoadSettings(DsmSettings settings)
    {
      var repositoryFile = getRepositoryFile();

      var settingsNode = getSettingsNode(repositoryFile);

      loadSetting("AskConfirmationOnDelete", (string s) => settings.AskConfirmationOnDelete = bool.Parse(s), settingsNode);
      loadSetting("AskConfirmationOnLoad", (string s) => settings.AskConfirmationOnLoad = bool.Parse(s), settingsNode);
      loadSetting("AskConfirmationOnReload", (string s) => settings.AskConfirmationOnReload = bool.Parse(s), settingsNode);
      loadSetting("AskConfirmationOnSave", (string s) => settings.AskConfirmationOnSave = bool.Parse(s), settingsNode);
      loadSetting("AskConfirmationOnSaveAs", (string s) => settings.AskConfirmationOnSaveAs = bool.Parse(s), settingsNode);
      loadSetting("AskConfirmationRestoreDocs", (string s) => settings.AskConfirmationRestoreDocs = bool.Parse(s), settingsNode);
      loadSetting("RestoreOpenedDocumentsAfterDebug", (string s) => settings.RestoreOpenedDocumentsAfterDebug = bool.Parse(s), settingsNode);
      loadSetting("ToolBarRowIndex", (string s) => settings.ToolBarRowIndex = int.Parse(s), settingsNode);
      loadSetting("ToolBarVisible", (string s) => settings.ToolBarVisible = bool.Parse(s), settingsNode);
      loadSetting("ToolBarPosition", (string s) => settings.ToolBarPosition = s, settingsNode);
      loadSetting("ToolBarLeft", (string s) => settings.ToolBarLeft = int.Parse(s), settingsNode);
      loadSetting("ToolBarTop", (string s) => settings.ToolBarTop = int.Parse(s), settingsNode);
      //loadSetting("ToolBarWidth", (string s) => settings.ToolBarWidth = int.Parse(s), settingsNode);
      //loadSetting("ToolBarHeight", (string s) => settings.ToolBarHeight = int.Parse(s), settingsNode);
    }

    private XmlNode getSettingsNode(XmlDocument repositoryFile)
    {
      for (int i = 0; i < repositoryFile.ChildNodes.Count; i++)
      {
        if (repositoryFile.ChildNodes[i].Name.Equals(TAG_SETTINGS))
        {
          return repositoryFile.ChildNodes[i];
        }
      }

      return null;
    }

    private void loadSetting(string name, Action<string> assignSettingAction, XmlNode settingsNode)
    {
      for (int i = 0; i < settingsNode.ChildNodes.Count; i++)
      {
        if (settingsNode.ChildNodes[i].Name.Equals(name))
        {
          assignSettingAction(settingsNode.ChildNodes[i].InnerText);
          return;
        }
      }
    }

    public void SaveSettings(DsmSettings settings)
    {
      var repositoryFile = getRepositoryFile();

      var settingsNode = getSettingsNode(repositoryFile);

      saveSetting("AskConfirmationOnDelete", settings.AskConfirmationOnDelete.ToString(), settingsNode);
      saveSetting("AskConfirmationOnLoad", settings.AskConfirmationOnLoad.ToString(), settingsNode);
      saveSetting("AskConfirmationOnReload", settings.AskConfirmationOnReload.ToString(), settingsNode);
      saveSetting("AskConfirmationOnSave", settings.AskConfirmationOnSave.ToString(), settingsNode);
      saveSetting("AskConfirmationOnSaveAs", settings.AskConfirmationOnSaveAs.ToString(), settingsNode);
      saveSetting("AskConfirmationRestoreDocs", settings.AskConfirmationRestoreDocs.ToString(), settingsNode);
      saveSetting("RestoreOpenedDocumentsAfterDebug", settings.RestoreOpenedDocumentsAfterDebug.ToString(), settingsNode);
      saveSetting("ToolBarRowIndex", settings.ToolBarRowIndex.ToString(), settingsNode);
      saveSetting("ToolBarVisible", settings.ToolBarVisible.ToString(), settingsNode);
      saveSetting("ToolBarPosition", settings.ToolBarPosition.ToString(), settingsNode);
      saveSetting("ToolBarLeft", settings.ToolBarLeft.ToString(), settingsNode);
      saveSetting("ToolBarTop", settings.ToolBarTop.ToString(), settingsNode);
      //saveSetting("ToolBarWidth", settings.ToolBarWidth.ToString(), settingsNode);
      //saveSetting("ToolBarHeight", settings.ToolBarHeight.ToString(), settingsNode);

      repositoryFile.Save(FilePath);
    }

    private void saveSetting(string name, string value, XmlNode settingsNode)
    {
      for (int i = 0; i < settingsNode.ChildNodes.Count; i++)
      {
        if (settingsNode.ChildNodes[i].Name.Equals(name))
        {
          settingsNode.ChildNodes[i].InnerText = value;
          return;
        }
      }

      var newNode = settingsNode.OwnerDocument.CreateElement("", name, "");
      newNode.InnerText = value;
      settingsNode.AppendChild(newNode);
    }
  }
}
