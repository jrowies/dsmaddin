using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DocumentSessionManager;
using DocumentSessionManagerTests.Builders;

namespace DocumentSessionManagerTests
{
  [TestFixture]
  public class SessionTests
  {
    [TestCase]
    public void TestSimpleAddRemoveDocument()
    {
      var session = new SessionBuilder().StartDefault().Build();
      session.AddDocument(new SessionDocumentBuilder().StartDefault("path1").Build());
      session.AddDocument(new SessionDocumentBuilder().StartDefault("path2").Build());

      var document = new SessionDocumentBuilder().StartDefault("path3").Build();
      session.AddDocument(document);
      session.RemoveDocument(document);

      Assert.AreEqual(2, session.GetDocuments().Count());
    }

    [TestCase, ExpectedException(typeof(ArgumentNullException))]
    public void TestAddNullDocument()
    {
      var session = new SessionBuilder().StartDefault().Build();
      session.AddDocument(null);
    }

    [TestCase, ExpectedException(typeof(ArgumentNullException))]
    public void TestRemoveNullDocument()
    {
      var session = new SessionBuilder().StartDefault().Build();
      session.RemoveDocument(null);
    }

    [TestCase]
    public void TestAddDuplicatedDocument()
    {
      var session = new SessionBuilder().StartDefault().Build();

      Assert.DoesNotThrow(delegate()
      {
        session.AddDocument(new SessionDocumentBuilder().StartDefault().Build());
        session.AddDocument(new SessionDocumentBuilder().StartDefault().Build());
      });

      Assert.AreEqual(1, session.GetDocuments().Count());
    }

    [TestCase]
    public void TestRemoveNonExistentDocument()
    {
      var session = new SessionBuilder().StartDefault().Build();

      Assert.DoesNotThrow(delegate() 
      {
        session.RemoveDocument(new SessionDocumentBuilder().StartDefault().Build());
      });
    }

    [TestCase]
    public void TestIsDirty()
    {
      var session = new SessionBuilder().StartDefault().Build();
      var document = new SessionDocumentBuilder().StartDefault().Build();
      session.AddDocument(document);
      session.IsDirty = false;

      session.RemoveDocument(document);

      Assert.IsTrue(session.IsDirty);
    }

    [TestCase]
    public void TestRemoveAllDocuments()
    {
      var session = new SessionBuilder().StartDefault().Build();
      session.IsDirty = false;

      session.RemoveAllDocuments();
      Assert.IsFalse(session.IsDirty);

      var document = new SessionDocumentBuilder().StartDefault().Build();
      session.AddDocument(document);
      session.IsDirty = false;

      session.RemoveAllDocuments();

      Assert.AreEqual(0, session.GetDocuments().Count());
      Assert.IsTrue(session.IsDirty);
    }

  }
}
