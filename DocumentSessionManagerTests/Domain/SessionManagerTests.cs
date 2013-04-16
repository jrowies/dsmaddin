using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DocumentSessionManager;
using Moq;
using DocumentSessionManagerTests.Builders;

namespace DocumentSessionManagerTests
{
  [TestFixture]
  public class SessionManagerTests
  {
    [SetUp]
    public void SetUp()
    {
    }

    [TestCase]
    public void TestSimpleAddRemoveSession()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      Session newSession = new SessionBuilder().StartDefault().Build();
      manager.AddSession(newSession);

      Assert.AreEqual(1, manager.GetSessions().Count());
      Assert.AreSame(newSession, manager.GetSessions().First());

      manager.RemoveSession(newSession);

      Assert.AreEqual(0, manager.GetSessions().Count());
    }

    [TestCase, ExpectedException(typeof(Exception))]
    public void TestAddDuplicatedSession()
    {
      const string DUPLICATED_NAME = "S1";

      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      Session newSession = new SessionBuilder().StartDefault(DUPLICATED_NAME).Build();
      manager.AddSession(newSession);
      Session newSession2 = new SessionBuilder().StartDefault(DUPLICATED_NAME).Build();
      manager.AddSession(newSession2);
    }

    [TestCase, ExpectedException(typeof(Exception))]
    public void TestRemoveNonExistentSession()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      Session newSession = new SessionBuilder().StartDefault().Build();

      manager.RemoveSession(newSession);
    }

    [TestCase, ExpectedException(typeof(ArgumentNullException))]
    public void TestAddNullSession()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      manager.AddSession(null);
    }

    [TestCase, ExpectedException(typeof(ArgumentNullException))]
    public void TestRemoveNullSession()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      manager.RemoveSession(null);
    }

    [TestCase]
    public void TestSetCurrentSessionByName()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      Session newSession = new SessionBuilder().StartDefault().Build();
      manager.AddSession(newSession);

      Assert.IsNull(manager.CurrentSession);

      bool result = manager.SetCurrentSessionByName(newSession.Name);

      Assert.AreSame(newSession, manager.CurrentSession);
      Assert.IsTrue(result);

      result = manager.SetCurrentSessionByName(newSession.Name);
      Assert.IsFalse(result);
    }

    [TestCase]
    public void TestGetSessionNames()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      Session newSession = new SessionBuilder().StartDefault("S1").Build();
      manager.AddSession(newSession);

      newSession = new SessionBuilder().StartDefault("S2").Build();
      manager.AddSession(newSession);

      newSession = new SessionBuilder().StartDefault("S3").Build();
      manager.AddSession(newSession);

      var names = manager.GetSessionNames();

      const int QTY_SESSIONS_ADDED = 3;

      Assert.AreEqual(QTY_SESSIONS_ADDED, names.Count());

      Assert.AreEqual(1, names.Where(n => n.Equals("S1")).Count());
      Assert.AreEqual(1, names.Where(n => n.Equals("S2")).Count());
      Assert.AreEqual(1, names.Where(n => n.Equals("S3")).Count());
    }

    [TestCase]
    public void TestGetSession()
    {
      var mock = new Mock<ISessionRepository>();
      var manager = new SessionManager(mock.Object);

      Session otherSession = new SessionBuilder().StartDefault("S1").Build();
      Session expectedSession = new SessionBuilder().StartDefault("S2").Build();

      manager.AddSession(otherSession);
      manager.AddSession(expectedSession);

      var actualSession = manager.GetSession(expectedSession.Name);

      Assert.AreSame(expectedSession, actualSession);
    }

    [TestCase]
    public void TestLoad()
    {
      Session s1 = new SessionBuilder().StartDefault("S1").Build();
      Session s2 = new SessionBuilder().StartDefault("S2").Build();
      Session s3 = new SessionBuilder().StartDefault("S3").Build();

      var mock = new Mock<ISessionRepository>();
      mock.Setup(repo => repo.GetAllSessions()).Returns(new List<Session>() { s1, s2, s3 });
      mock.Setup(repo => repo.GetCurrentSessionName()).Returns("S2");

      var manager = new SessionManager(mock.Object);

      manager.Load();

      var sessions = manager.GetSessions();

      Assert.AreEqual(3, sessions.Count());

      Assert.AreEqual(1, sessions.Where(s => s.Equals(s1)).Count());
      Assert.AreEqual(1, sessions.Where(s => s.Equals(s2)).Count());
      Assert.AreEqual(1, sessions.Where(s => s.Equals(s3)).Count());

      Assert.AreSame(s2, manager.CurrentSession);
    }

    [TestCase]
    public void TestPersist()
    {
      Session s1 = new SessionBuilder().StartDefault("S1").Build();
      Session s2 = new SessionBuilder().StartDefault("S2").Build();
      Session s3 = new SessionBuilder().StartDefault("S3").Build();

      var mockUnitOfWork = new Mock<IUnitOfWork>();
      var mockRepository = new Mock<ISessionRepository>();
      mockRepository.Setup(sm => sm.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

      var manager = new SessionManager(mockRepository.Object);
      manager.AddSession(s1);
      manager.AddSession(s2);
      manager.AddSession(s3);
      manager.RemoveSession(s3);
      manager.CurrentSession = s1;

      manager.Persist();

      mockUnitOfWork.Verify(uow => uow.RegisterSavedOrUpdated(It.Is<Session>(o => o.Equals(s1))), Times.Once());
      mockUnitOfWork.Verify(uow => uow.RegisterSavedOrUpdated(It.Is<Session>(o => o.Equals(s2))), Times.Once());
      mockUnitOfWork.Verify(uow => uow.RegisterRemoved(It.Is<Session>(o => o.Equals(s3))), Times.Once());
      mockUnitOfWork.Verify(uow => uow.RegisterSavedOrUpdated(It.Is<CurrentSessionInfo>(o => o.CurrentSessionName.Equals(s1.Name))), Times.Once());
      mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once());
    }

    private const string STR_ExceptionOnCommitForTestingPurposes = "exception on commit for testing purposes";

    [TestCase, ExpectedException(ExpectedMessage = STR_ExceptionOnCommitForTestingPurposes)]
    public void TestPersistWithRollback()
    {
      Session s1 = new SessionBuilder().StartDefault("S1").Build();

      var mockUnitOfWork = new Mock<IUnitOfWork>();
      mockUnitOfWork.Setup(uow => uow.Commit()).Throws(new Exception(STR_ExceptionOnCommitForTestingPurposes));

      var mockRepository = new Mock<ISessionRepository>();
      mockRepository.Setup(sm => sm.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

      var manager = new SessionManager(mockRepository.Object);
      manager.AddSession(s1);

      manager.Persist();

      mockUnitOfWork.Verify(uow => uow.Rollback(), Times.Once());
    }
  }
}
