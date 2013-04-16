using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using EnvDTE80;
using EnvDTE;
using System.IO;

namespace DocumentSessionManager
{
  public class DteAdapter : IDteAdapter
  {
    private DteAdapter()
    {
    }

    public DteAdapter(DTE2 dteApplicationObject)
    {
      _dteApplicationObject = dteApplicationObject;
    }

    private DTE2 _dteApplicationObject;

    public IEnumerable<IDteWindowAdapter> GetWindowsForValidDocuments()
    {
      var result = new List<IDteWindowAdapter>();

      IEnumerator enumerator = _dteApplicationObject.Windows.GetEnumerator();
      while (enumerator.MoveNext())
      {
        var currWindow = enumerator.Current as Window;

        if (windowBelongsToValidDocument(currWindow))
          result.Add(new DteWindowAdapter(currWindow));
      }

      return result;
    }

    private static bool windowBelongsToValidDocument(Window window)
    {
      return ((window.Type == vsWindowType.vsWindowTypeDocument) || (window.Type == vsWindowType.vsWindowTypeDesigner)) &&
                  (window.Document != null) && (window.Visible);
    }

    public void OpenFile(string fullPath, DocumentType type)
    {
      _dteApplicationObject.ItemOperations.OpenFile(fullPath, documentTypeToDTEType(type));
    }

    private static string documentTypeToDTEType(DocumentType type)
    {
      if (type == DocumentType.Designer)
        return EnvDTE.Constants.vsViewKindDesigner;
      else if (type == DocumentType.Text)
        return EnvDTE.Constants.vsViewKindCode;
      else
        return EnvDTE.Constants.vsViewKindPrimary;
    }

    public bool FileExists(string path)
    {
      return File.Exists(path);
    }
  }
}