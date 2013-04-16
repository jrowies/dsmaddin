using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.Xml;
using System.IO;
using DocumentSessionManagerTests.Builders;
using DocumentSessionManager;

namespace DocumentSessionManagerTests
{
  [TestFixture]
  public class XmlSettingsRepositoryTests
  {
    [SetUp]
    public void SetUp()
    {
      string file = XmlSettingsRepository.FilePath;

      if (File.Exists(file))
        File.Delete(file);
    }

    [TearDown]
    public void TearDown()
    {
      string file = XmlSettingsRepository.FilePath;
      if (File.Exists(file))
        File.Delete(file);
    }

    [TestCase]
    public void TestSettingsDefaultValuesAreCorrect()
    {
      var settings = new DsmSettings();

      Assert.IsTrue(settings.AskConfirmationOnDelete);
      Assert.IsTrue(settings.AskConfirmationOnLoad);
      Assert.IsTrue(settings.AskConfirmationOnReload);
      Assert.IsTrue(settings.AskConfirmationOnSave);
      Assert.IsTrue(settings.AskConfirmationOnSaveAs);
      Assert.IsTrue(settings.AskConfirmationRestoreDocs);
      Assert.IsTrue(settings.RestoreOpenedDocumentsAfterDebug);
      Assert.AreEqual(10, settings.ToolBarRowIndex);
      Assert.IsTrue(settings.ToolBarVisible);
      Assert.AreEqual(-1, settings.ToolBarTop);
      Assert.AreEqual(-1, settings.ToolBarLeft);
      Assert.IsNull(settings.ToolBarPosition);
    }

    [TestCase]
    public void TestSaveAndLoadSettings()
    {
      var repo = new XmlSettingsRepository();
      var settings = new DsmSettings();
      repo.LoadSettings(settings);

      Assert.IsTrue(settings.AskConfirmationOnDelete);
      Assert.IsTrue(settings.AskConfirmationOnLoad);
      Assert.IsTrue(settings.AskConfirmationOnReload);
      Assert.IsTrue(settings.AskConfirmationOnSave);
      Assert.IsTrue(settings.AskConfirmationOnSaveAs);
      Assert.IsTrue(settings.AskConfirmationRestoreDocs);
      Assert.IsTrue(settings.RestoreOpenedDocumentsAfterDebug);
      Assert.AreEqual(10, settings.ToolBarRowIndex);
      Assert.IsTrue(settings.ToolBarVisible);
      Assert.AreEqual(-1, settings.ToolBarTop);
      Assert.AreEqual(-1, settings.ToolBarLeft);
      Assert.IsNull(settings.ToolBarPosition);

      settings.AskConfirmationOnDelete = false;
      settings.AskConfirmationOnLoad = false;
      settings.AskConfirmationOnReload = false;
      settings.AskConfirmationOnSave = false;
      settings.AskConfirmationOnSaveAs = false;
      settings.AskConfirmationRestoreDocs = false;
      settings.RestoreOpenedDocumentsAfterDebug = false;
      settings.ToolBarRowIndex = 1;
      settings.ToolBarVisible = false;
      settings.ToolBarTop = 1;
      settings.ToolBarLeft = 1;
      settings.ToolBarPosition = "something";

      repo.SaveSettings(settings);

      settings = new DsmSettings();
      repo = new XmlSettingsRepository();
      repo.LoadSettings(settings);

      Assert.IsFalse(settings.AskConfirmationOnDelete);
      Assert.IsFalse(settings.AskConfirmationOnLoad);
      Assert.IsFalse(settings.AskConfirmationOnReload);
      Assert.IsFalse(settings.AskConfirmationOnSave);
      Assert.IsFalse(settings.AskConfirmationOnSaveAs);
      Assert.IsFalse(settings.AskConfirmationRestoreDocs);
      Assert.IsFalse(settings.RestoreOpenedDocumentsAfterDebug);
      Assert.AreEqual(1, settings.ToolBarRowIndex);
      Assert.IsFalse(settings.ToolBarVisible);
      Assert.AreEqual(1, settings.ToolBarTop);
      Assert.AreEqual(1, settings.ToolBarLeft);
      Assert.AreEqual("something", settings.ToolBarPosition);
    }
  }
}
