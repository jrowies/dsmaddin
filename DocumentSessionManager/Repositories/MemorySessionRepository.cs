using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace DocumentSessionManager
{
  public class MemorySessionRepository : ISessionRepository
  {
    #region ISessionRepository Members

    public IEnumerable<Session> GetAllSessions()
    {
      var sessions = new Collection<Session>();

      var s1 = new Session("Class1 + Class2");
      s1.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary2\Class1.cs", DocumentType.Text));
      s1.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary1\Class2.cs", DocumentType.Text));
      sessions.Add(s1);

      var s2 = new Session("Class1");
      s2.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary2\Class1.cs", DocumentType.Text));
      s2.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary2\pepe.cs", DocumentType.Text));
      sessions.Add(s2);

      var s3 = new Session("Class2 + Form1");
      s3.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary1\Class2.cs", DocumentType.Text));
      s3.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary1\Form1.cs", DocumentType.Designer));
      s3.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary1\Form1.cs", DocumentType.Text));
      s3.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary1\XMLFile1.xml", DocumentType.Text));
      s3.AddDocument(new SessionDocument(@"c:\VSProjects\Prueba1\ClassLibrary1\ClassDiagram1.cd", DocumentType.Text));
      sessions.Add(s3);

      return sessions;
    }

    public string GetCurrentSessionName()
    {
      return "Class1 + Class2";
    }

    #endregion

    #region IRepository Members

    public void Persist(IList<object> savedOrUpdated, IList<object> removed)
    {
    }

    public IUnitOfWork GetUnitOfWork()
    {
      return new UnitOfWork(this);
    }

    #endregion
  }
}
