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
  public class XmlSessionRepositoryTests
  {
    [SetUp]
    public void SetUp()
    {
      string file = XmlSessionRepository.FilePath;

      if (File.Exists(file))
        File.Delete(file);
    }

    [TearDown]
    public void TearDown()
    {
      string file = XmlSessionRepository.FilePath;
      if (File.Exists(file))
        File.Delete(file);
    }

    [TestCase]
    public void TestSaveMoreThanOneSession()
    {
      var repo = new XmlSessionRepository();
      var uow = repo.GetUnitOfWork();
      var session1 = new SessionBuilder().StartDefault("session1").Build();
      var session2 = new SessionBuilder().StartDefault("session2").Build();
      uow.RegisterSavedOrUpdated(session1);
      uow.RegisterSavedOrUpdated(session2);
      uow.Commit();

      repo = new XmlSessionRepository();
      var sessionsInRepo = repo.GetAllSessions();
      Assert.AreEqual(2, sessionsInRepo.Count());
      Assert.IsNotNull(sessionsInRepo.FirstOrDefault(s => s.Equals(session1)));
      Assert.IsNotNull(sessionsInRepo.FirstOrDefault(s => s.Equals(session2)));
    }

    [TestCase]
    public void TestSaveDocuments()
    {
      var repo = new XmlSessionRepository();
      var uow = repo.GetUnitOfWork();
      var session1 = new SessionBuilder().StartDefault("session1").Build();
      var doc1 = new SessionDocumentBuilder().StartDefault("doc1").Build();
      session1.AddDocument(doc1);
      var doc2 = new SessionDocumentBuilder().StartDefault("doc2").Build();
      session1.AddDocument(doc2);
      uow.RegisterSavedOrUpdated(session1);
      uow.Commit();

      repo = new XmlSessionRepository();
      var sessionsInRepo = repo.GetAllSessions();
      Assert.AreEqual(1, sessionsInRepo.Count());
      Assert.IsNotNull(sessionsInRepo.FirstOrDefault(s => s.Equals(session1)));
      Assert.AreEqual(2, sessionsInRepo.First().GetDocuments().Count());

      repo = new XmlSessionRepository();
      uow = repo.GetUnitOfWork();
      session1.RemoveDocument(doc1);
      uow.RegisterSavedOrUpdated(session1);
      uow.Commit();

      repo = new XmlSessionRepository();
      sessionsInRepo = repo.GetAllSessions();
      Assert.AreEqual(1, sessionsInRepo.Count());
      Assert.IsNotNull(sessionsInRepo.FirstOrDefault(s => s.Equals(session1)));
      Assert.AreEqual(1, sessionsInRepo.First().GetDocuments().Count());

    }

    [TestCase]
    public void TestCrud()
    {
      var repo = new XmlSessionRepository();
      var uow = repo.GetUnitOfWork();
      var session1 = new SessionBuilder().StartDefault("session1").Build();
      uow.RegisterSavedOrUpdated(session1);
      uow.Commit();

      repo = new XmlSessionRepository();
      var sessionsInRepo = repo.GetAllSessions();
      Assert.AreEqual(1, sessionsInRepo.Count());
      Assert.IsNotNull(sessionsInRepo.FirstOrDefault(s => s.Equals(session1)));

      repo = new XmlSessionRepository();
      uow = repo.GetUnitOfWork();
      session1.AddDocument(new SessionDocumentBuilder().StartDefault("doc1").Build()); // <-- adding one document to modify something in the session
      uow.RegisterSavedOrUpdated(session1);
      uow.Commit();

      repo = new XmlSessionRepository();
      sessionsInRepo = repo.GetAllSessions();
      Assert.AreEqual(1, sessionsInRepo.Count());
      Assert.AreEqual(1, sessionsInRepo.First().GetDocuments().Count());

      repo = new XmlSessionRepository();
      uow = repo.GetUnitOfWork();
      uow.RegisterRemoved(session1);
      uow.Commit();

      repo = new XmlSessionRepository();
      sessionsInRepo = repo.GetAllSessions();
      Assert.AreEqual(0, sessionsInRepo.Count());

    }
  }
}
