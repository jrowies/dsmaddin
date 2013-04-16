using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DocumentSessionManager;
using Moq;
using DocumentSessionManagerTests.Builders;
using System.Collections;

namespace DocumentSessionManagerTests
{
  [TestFixture]
  public class AddinControllerTests
  {
    [TestCase]
    public void TestFillDocumentsInSession()
    {
      const string DOCUMENT_TO_ADD1 = "documentToAdd1";
      const string DOCUMENT_TO_ADD2 = "documentToAdd2";
      const string DOCUMENT_TO_ADD3 = "documentToAdd3";
      const string DOCUMENT_DUPLICATED = "documentDuplicated";

      var session = new SessionBuilder().StartDefault().Build();
      session.AddDocument(new SessionDocumentBuilder().StartDefault("documentToRemove").Build());
      session.AddDocument(new SessionDocumentBuilder().StartDefault(DOCUMENT_DUPLICATED).Build());

      var dteWindowAdapterMock1 = new Mock<IDteWindowAdapter>();
      dteWindowAdapterMock1.Setup(dwa => dwa.FullPath).Returns(DOCUMENT_TO_ADD1);
      dteWindowAdapterMock1.Setup(dwa => dwa.DocumentType).Returns(DocumentType.Text);

      var dteWindowAdapterMock2 = new Mock<IDteWindowAdapter>();
      dteWindowAdapterMock2.Setup(dwa => dwa.FullPath).Returns(DOCUMENT_TO_ADD2);
      dteWindowAdapterMock2.Setup(dwa => dwa.DocumentType).Returns(DocumentType.Text);

      var dteWindowAdapterMock3 = new Mock<IDteWindowAdapter>();
      dteWindowAdapterMock3.Setup(dwa => dwa.FullPath).Returns(DOCUMENT_TO_ADD3);
      dteWindowAdapterMock3.Setup(dwa => dwa.DocumentType).Returns(DocumentType.Text);

      var dteWindowAdapterMock4 = new Mock<IDteWindowAdapter>();
      dteWindowAdapterMock4.Setup(dwa => dwa.FullPath).Returns(DOCUMENT_DUPLICATED);
      dteWindowAdapterMock4.Setup(dwa => dwa.DocumentType).Returns(DocumentType.Text);

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(
        new List<IDteWindowAdapter>() 
        { 
          dteWindowAdapterMock1.Object, 
          dteWindowAdapterMock2.Object, 
          dteWindowAdapterMock3.Object,
          dteWindowAdapterMock4.Object
        });

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns((Session)null);

      var controller = new AddinController(sessionManagerMock.Object, null, null, dteAdapterMock.Object, null);
      controller.FillDocumentsInSession(session);

      IEnumerable<SessionDocument> documentsInSession = session.GetDocuments();

      Assert.AreEqual(4, documentsInSession.Count());
      Assert.AreEqual(1, documentsInSession.Where(d => d.Path.Equals(DOCUMENT_TO_ADD1)).Count());
      Assert.AreEqual(1, documentsInSession.Where(d => d.Path.Equals(DOCUMENT_TO_ADD2)).Count());
      Assert.AreEqual(1, documentsInSession.Where(d => d.Path.Equals(DOCUMENT_TO_ADD3)).Count());
      Assert.AreEqual(1, documentsInSession.Where(d => d.Path.Equals(DOCUMENT_DUPLICATED)).Count());
    }

    [TestCase]
    public void TestSaveSession()
    {
      var sessionToSave = new SessionBuilder().StartDefault().Build();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns(sessionToSave);

      var dsmSettings = new DsmSettingsBuilder().StartDefault().Build();
      var settingsManagerMock = new Mock<IDsmSettingsManager>();
      settingsManagerMock.Setup(st => st.DsmSettings).Returns(dsmSettings);

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>());

      var controller = new AddinController(sessionManagerMock.Object, settingsManagerMock.Object, null, dteAdapterMock.Object, null);
      controller.SaveSession();

      sessionManagerMock.Verify(sm => sm.Persist(), Times.Once());
    }

    [TestCase]
    public void TestSaveSessionAsWithReplace()
    {
      const string SESSION_TO_REPLACE_NAME = "SESSION_TO_REPLACE_NAME";

      var sessionToReplace = new SessionBuilder().StartDefault(SESSION_TO_REPLACE_NAME).Build();
      var sessions = new List<Session>() { sessionToReplace };

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.GetSessions()).Returns(sessions);
      sessionManagerMock.Setup(sm => sm.GetSession(SESSION_TO_REPLACE_NAME)).Returns(sessionToReplace);
      Session sessionToSave = null;
      sessionManagerMock.SetupSet(sm => sm.CurrentSession).Callback(delegate(Session s)
                                                                    {
                                                                      sessionToSave = s;
                                                                    });
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns(delegate()
                                                                {
                                                                  return sessionToSave;
                                                                });

      var viewAdapterMock = new Mock<IViewAdapter>();
      viewAdapterMock.Setup(va => va.GetSessionForSaveAs(It.IsAny<IList<SessionDto>>()))
        .Returns((IList<SessionDto> dtoList) => dtoList[0]); //<-- first session (sessionToReplace)

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>());

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object, dteAdapterMock.Object, null);
      controller.SaveSessionAs();

      sessionManagerMock.VerifySet(sm => sm.CurrentSession = sessionToSave, Times.Once());
      sessionManagerMock.Verify(sm => sm.Persist(), Times.Once());
    }

    [TestCase]
    public void TestSaveSessionAsWithNoReplace()
    {
      const string NEW_SESSION_NAME = "NEW_SESSION_NAME";

      var newSession = new SessionBuilder().StartDefault(NEW_SESSION_NAME).Build();
      var sessions = new List<Session>();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.GetSessions()).Returns(sessions);
      sessionManagerMock.Setup(sm => sm.GetSession(NEW_SESSION_NAME)).Returns((Session)null);
      Session sessionToSave = null;
      sessionManagerMock.SetupSet(sm => sm.CurrentSession).Callback(delegate(Session s)
      {
        sessionToSave = s;
      });
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns(delegate()
      {
        return sessionToSave;
      });

      var viewAdapterMock = new Mock<IViewAdapter>();
      viewAdapterMock.Setup(va => va.GetSessionForSaveAs(It.IsAny<IList<SessionDto>>()))
        .Returns((IList<SessionDto> dtoList) => new SessionDto { Name = NEW_SESSION_NAME }); 

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>());

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object, dteAdapterMock.Object, null);
      controller.SaveSessionAs();

      sessionManagerMock.Verify(sm => sm.AddSession(It.Is<Session>(s => s.Name == NEW_SESSION_NAME)), Times.Once());
      sessionManagerMock.VerifySet(sm => sm.CurrentSession = sessionToSave, Times.Once());
      sessionManagerMock.Verify(sm => sm.Persist(), Times.Once());
    }

    [TestCase]
    public void TestReloadSession()
    {
      var sessionToReload = new SessionBuilder().StartDefault().Build();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns(sessionToReload);

      var dsmSettings = new DsmSettingsBuilder().StartDefault().Build();
      var settingsManagerMock = new Mock<IDsmSettingsManager>();
      settingsManagerMock.Setup(st => st.DsmSettings).Returns(dsmSettings);

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>());

      var controller = new AddinController(sessionManagerMock.Object, settingsManagerMock.Object, null, 
        dteAdapterMock.Object, null);
      controller.ReloadSession();

      sessionManagerMock.Verify(sm => sm.CurrentSession);
      settingsManagerMock.Verify(st => st.DsmSettings);
    }

    [TestCase]
    public void TestLoadDocumentsFromSession()
    {
      const string DOCUMENT_IN_SESSION_AND_IN_THE_IDE = "d1";
      const string DOCUMENT_IN_SESSION = "d2";
      const string DOCUMENT_IN_THE_IDE = "d3";

      var sessionWithDocumentsToLoad = new SessionBuilder().StartDefault().Build();
      
      var documentInSessionAndInTheIDE = new SessionDocumentBuilder().StartDefault(DOCUMENT_IN_SESSION_AND_IN_THE_IDE).Build();
      sessionWithDocumentsToLoad.AddDocument(documentInSessionAndInTheIDE);

      var documentInSession = new SessionDocumentBuilder().StartDefault(DOCUMENT_IN_SESSION).Build();
      sessionWithDocumentsToLoad.AddDocument(documentInSession);

      var dteWindowAdapterMock1 = new Mock<IDteWindowAdapter>();
      dteWindowAdapterMock1.Setup(dwa => dwa.FullPath).Returns(DOCUMENT_IN_SESSION_AND_IN_THE_IDE);
      dteWindowAdapterMock1.Setup(dwa => dwa.DocumentMatches(documentInSessionAndInTheIDE)).Returns(true);

      var dteWindowAdapterMock2 = new Mock<IDteWindowAdapter>();
      dteWindowAdapterMock2.Setup(dwa => dwa.FullPath).Returns(DOCUMENT_IN_THE_IDE);
      dteWindowAdapterMock2.Setup(dwa => dwa.DocumentMatches(It.IsAny<SessionDocument>())).Returns(false);
      dteWindowAdapterMock2.Setup(dwa => dwa.Close(It.Is<SaveChanges>(x => x == SaveChanges.Prompt))).Returns(true);

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>() 
      { 
        dteWindowAdapterMock1.Object,
        dteWindowAdapterMock2.Object,
      });

      dteAdapterMock.Setup(da => da.FileExists(It.IsAny<string>())).Returns(true);

      var viewAdapterMock = new Mock<IViewAdapter>();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns((Session)null);

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object,
        dteAdapterMock.Object, null);
      controller.LoadDocumentsFromSession(sessionWithDocumentsToLoad);

      dteAdapterMock.Verify(da => da.OpenFile(It.IsAny<string>(), It.IsAny<DocumentType>()), Times.Once());
    }

    [TestCase]
    public void TestLoadDocumentsFromSession_NonExistentDocument()
    {
      const string NON_EXISTENT_DOCUMENT = "NON_EXISTENT_DOCUMENT";

      var sessionWithDocumentsToLoad = new SessionBuilder().StartDefault().Build();

      var nonExistentDocument = new SessionDocumentBuilder().StartDefault(NON_EXISTENT_DOCUMENT).Build();
      sessionWithDocumentsToLoad.AddDocument(nonExistentDocument);

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>());

      dteAdapterMock.Setup(da => da.FileExists(It.IsAny<string>())).Returns(false);

      var viewAdapterMock = new Mock<IViewAdapter>();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns((Session)null);

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object,
        dteAdapterMock.Object, null);
      controller.LoadDocumentsFromSession(sessionWithDocumentsToLoad);

      dteAdapterMock.Verify(da => da.OpenFile(It.IsAny<string>(), It.IsAny<DocumentType>()), Times.Never());
      dteAdapterMock.Verify(da => da.FileExists(It.IsAny<string>()), Times.Once());
      viewAdapterMock.Verify(va => va.ShowLongMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
    }

    [TestCase]
    public void TestDeleteSessions()
    {
      var sessionToDelete = new SessionBuilder().StartDefault().Build();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.GetSessions()).Returns(new List<Session>());
      sessionManagerMock.Setup(sm => sm.GetSession(It.IsAny<string>())).Returns(sessionToDelete);
      sessionManagerMock.Setup(sm => sm.CurrentSession).Returns(sessionToDelete);

      var viewAdapterMock = new Mock<IViewAdapter>();
      viewAdapterMock.Setup(va => va.GetSessionsForDelete(It.IsAny<IList<SessionDto>>()))
        .Returns(new List<SessionDto>() 
        { 
          new SessionDto()
        });

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object,
        null, null);
      controller.DeleteSessions();

      sessionManagerMock.Verify(sm => sm.RemoveSession(sessionToDelete), Times.Once());
      sessionManagerMock.Verify(sm => sm.Persist(), Times.Once());
    }

    [TestCase]
    public void TestLoadSession()
    {
      var sessionToLoad = new SessionBuilder().StartDefault().Build();

      var sessionManagerMock = new Mock<ISessionManager>();
      sessionManagerMock.Setup(sm => sm.GetSessions()).Returns(new List<Session>());
      sessionManagerMock.Setup(sm => sm.GetSession(It.IsAny<string>())).Returns(sessionToLoad);

      var viewAdapterMock = new Mock<IViewAdapter>();
      viewAdapterMock.Setup(va => va.GetSessionForLoading(It.IsAny<IList<SessionDto>>())).Returns(new SessionDto());

      var dteAdapterMock = new Mock<IDteAdapter>();
      dteAdapterMock.Setup(da => da.GetWindowsForValidDocuments()).Returns(new List<IDteWindowAdapter>());

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object,
        dteAdapterMock.Object, null);
      controller.LoadSession();

      sessionManagerMock.VerifySet(sm => sm.CurrentSession = sessionToLoad, Times.Once());
    }

    [TestCase]
    public void TestRefreshCurrentSession()
    {
      var sessionManagerMock = new Mock<ISessionManager>();

      var controller = new AddinController(sessionManagerMock.Object, null, null,
        null, null);
      controller.RefreshCurrentSession();

      sessionManagerMock.Verify(sm => sm.SetCurrentSessionByName(It.IsAny<string>()));
    }

    private static void addDocumentsToList(int qty, List<SessionDocument> list)
    {
      addDocumentsToList(qty, list, string.Empty);
    }

    private static void addDocumentsToList(int qty, List<SessionDocument> list, string prefix)
    {
      for (int i = 1; i <= qty; i++)
      {
        list.Add(new SessionDocumentBuilder().StartDefault(string.Format("{0}_DOC_{1}", prefix, i)).Build());
      }
    }

    [TestCase]
    public void TestAddDocumentsToRecentlyClosedList_CapacityNotExeeded()
    {
      var recentlyClosedDocs = new List<SessionDocument>();
      var document1 = new SessionDocumentBuilder().StartDefault("A").Build();
      recentlyClosedDocs.Add(document1);

      var elementsToAdd = new List<SessionDocument>();
      var document2 = new SessionDocumentBuilder().StartDefault("A").Build();
      elementsToAdd.Add(document2);
      var document3 = new SessionDocumentBuilder().StartDefault("B").Build();
      elementsToAdd.Add(document3);

      AddinController.AddDocumentsToRecentlyClosedList(recentlyClosedDocs, elementsToAdd);

      Assert.AreEqual(2, recentlyClosedDocs.Count);
      Assert.Contains(document2, recentlyClosedDocs);
      Assert.Contains(document3, recentlyClosedDocs);
    }

    [TestCase]
    public void TestAddDocumentsToRecentlyClosedList_ElementsToAddExceedsCapacity()
    {
      var recentlyClosedDocs = new List<SessionDocument>();
      var document1 = new SessionDocumentBuilder().StartDefault("A").Build();
      recentlyClosedDocs.Add(document1);

      var elementsToAdd = new List<SessionDocument>();
      addDocumentsToList(AddinController.INT_RecentlyClosedDocsCapacity + 1, elementsToAdd);
 
      AddinController.AddDocumentsToRecentlyClosedList(recentlyClosedDocs, elementsToAdd);

      Assert.AreEqual(AddinController.INT_RecentlyClosedDocsCapacity + 1, recentlyClosedDocs.Count);
      Assert.IsFalse(recentlyClosedDocs.Contains(document1));
    }

    [TestCase]
    public void TestAddDocumentsToRecentlyClosedList_ElementsToAddPlusRecentlyClosedExceedsCapacity()
    {
      var recentlyClosedDocs = new List<SessionDocument>();
      addDocumentsToList((AddinController.INT_RecentlyClosedDocsCapacity / 2) + 1, recentlyClosedDocs, "RECENTLY_CLOSED_DOCS");

      var elementsToAdd = new List<SessionDocument>();
      addDocumentsToList((AddinController.INT_RecentlyClosedDocsCapacity / 2) + 1, elementsToAdd, "ELEMENTS_TO_ADD");

      AddinController.AddDocumentsToRecentlyClosedList(recentlyClosedDocs, elementsToAdd);

      Assert.AreEqual(AddinController.INT_RecentlyClosedDocsCapacity, recentlyClosedDocs.Count);
      Assert.AreEqual((AddinController.INT_RecentlyClosedDocsCapacity / 2) + 1, recentlyClosedDocs.Count(d => d.Path.StartsWith("ELEMENTS_TO_ADD")));
    }

    [TestCase]
    public void TestShowRecentlyClosedDocuments()
    {
      const int ELEMENTS_TO_ADD = 5;

      var elementsToAdd = new List<SessionDocument>();
      addDocumentsToList(ELEMENTS_TO_ADD, elementsToAdd);

      var sessionManagerMock = new Mock<ISessionManager>();

      var viewAdapterMock = new Mock<IViewAdapter>();

      var controller = new AddinController(sessionManagerMock.Object, null, viewAdapterMock.Object, null, null);
      controller.AddDocumentsToRecentlyClosedList(elementsToAdd);
      controller.ShowRecentlyClosedDocuments();

      viewAdapterMock.Verify(va => va.ShowRecentlyClosedDocuments(It.Is<List<SessionDocumentDto>>(x => x.Count == ELEMENTS_TO_ADD)));
    }
  }
}
