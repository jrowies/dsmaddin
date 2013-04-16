using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class DtoMapper
  {
    public static void MapObjToDto(SessionDocument obj, SessionDocumentDto dto)
    {
      dto.Path = obj.Path;
      dto.Type = obj.Type;
    }

    public static void MapObjToDto(Session obj, SessionDto dto)
    {
      dto.Name = obj.Name;
      foreach (var doc in obj.GetDocuments())
      {
        var dtoDoc = new SessionDocumentDto();
        DtoMapper.MapObjToDto(doc, dtoDoc);
        dto.Documents.Add(dtoDoc);
      }
    }
  }
}
