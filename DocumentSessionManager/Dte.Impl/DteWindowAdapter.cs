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
  public class DteWindowAdapter : IDteWindowAdapter
  {
    private Window _dteWindow;

    public DteWindowAdapter(Window dteWindow)
    {
      _dteWindow = dteWindow;
    }

    public string FullPath
    {
      get
      {
        return Path.Combine(_dteWindow.Document.Path, _dteWindow.Document.Name);
      }
    }

    public DocumentType DocumentType
    {
      get
      {
        if (_dteWindow.Object is System.ComponentModel.Design.IDesignerHost)
        {
          //in some cases, even when designer is shown, windowDTE.Type is vsWindowType.vsWindowTypeDocument, so I have to do this
          return DocumentType.Designer;
        }
        else if (_dteWindow.Type == vsWindowType.vsWindowTypeDocument)
          return DocumentType.Text;
        else
          return DocumentType.Text;
      }
    }

    public bool DocumentMatches(SessionDocument document)
    {
      return (this.DocumentType == document.Type) &&
        (this.FullPath.Equals(document.Path, StringComparison.OrdinalIgnoreCase));
    }

    public bool Close(SaveChanges saveChanges)
    {
      switch (saveChanges)
      {
      	case SaveChanges.No:
          _dteWindow.Close(vsSaveChanges.vsSaveChangesNo);
      		break;
        case SaveChanges.Yes:
          _dteWindow.Close(vsSaveChanges.vsSaveChangesYes);
          break;
        case SaveChanges.Prompt:
          try
          {
            _dteWindow.Close(vsSaveChanges.vsSaveChangesPrompt);
          }
          catch //user cancelled the operation
          {
            return false; 
          }
          break;
      }

      return true;
    }

  }
}
