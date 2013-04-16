using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DocumentSessionManager;

namespace DocumentSessionManagerTests
{
  [TestFixture]
  public class DtoMapperTests
  {
    [TestCase]
    public void TestSessionDocumentMapObjToDto()
    {
      var sessionDocument = new SessionDocument("path", DocumentType.Designer);
      var sessionDocumentDto = new SessionDocumentDto();

      DtoMapper.MapObjToDto(sessionDocument, sessionDocumentDto);

      Assert.AreEqual(sessionDocument.Path, sessionDocumentDto.Path);
      Assert.AreEqual(sessionDocument.Type, sessionDocumentDto.Type);
    }
    
    [TestCase]
    public void TestSessionMapObjToDto()
    {
      var session = new Session("session");
      var sessionDocument = new SessionDocument("path", DocumentType.Designer);
      session.AddDocument(sessionDocument);

      var sessionDto = new SessionDto();

      DtoMapper.MapObjToDto(session, sessionDto);

      Assert.AreEqual(session.Name, sessionDto.Name);

      Assert.AreEqual(session.GetDocuments().Count(), sessionDto.DocumentsCount);

      Assert.AreEqual(sessionDocument.Path, sessionDto.Documents[0].Path);
      Assert.AreEqual(sessionDocument.Type, sessionDto.Documents[0].Type);
    }
  }
}

