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
  public class SessionDocumentTests
  {
    [TestCase]
    public void TestEquals()
    {
      var d1 = new SessionDocumentBuilder().StartDefault("path", DocumentType.Designer).Build();
      var d2 = new SessionDocumentBuilder().StartDefault("path", DocumentType.Text).Build();
      var d3 = new SessionDocumentBuilder().StartDefault("path2", DocumentType.Text).Build();
      var d4 = new SessionDocumentBuilder().StartDefault("path", DocumentType.Text).Build();

      Assert.AreNotEqual(d1, d2);
      Assert.AreNotEqual(d1, d3);
      Assert.AreNotEqual(d2, d3);
      Assert.AreNotEqual(d3, d4);
      Assert.AreEqual(d2, d4);
    }
  }
}
