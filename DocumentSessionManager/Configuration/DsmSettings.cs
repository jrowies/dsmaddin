using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentSessionManager
{
  public class DsmSettings
  {
    private bool _askConfirmationOnSave = true;
    public bool AskConfirmationOnSave
    {
      get
      {
        return _askConfirmationOnSave;
      }
      set
      {
        _askConfirmationOnSave = value;
      }
    }

    private bool _askConfirmationOnSaveAs = true;
    public bool AskConfirmationOnSaveAs
    {
      get
      {
        return _askConfirmationOnSaveAs;
      }
      set
      {
        _askConfirmationOnSaveAs = value;
      }
    }

    private bool _askConfirmationOnLoad = true;
    public bool AskConfirmationOnLoad
    {
      get
      {
        return _askConfirmationOnLoad;
      }
      set
      {
        _askConfirmationOnLoad = value;
      }
    }

    private bool _askConfirmationOnDelete = true;
    public bool AskConfirmationOnDelete
    {
      get
      {
        return _askConfirmationOnDelete;
      }
      set
      {
        _askConfirmationOnDelete = value;
      }
    }

    private bool _askConfirmationOnReload = true;
    public bool AskConfirmationOnReload
    {
      get
      {
        return _askConfirmationOnReload;
      }
      set
      {
        _askConfirmationOnReload = value;
      }
    }

    private bool _askConfirmationRestoreDocs = true;
    public bool AskConfirmationRestoreDocs
    {
      get
      {
        return _askConfirmationRestoreDocs;
      }
      set
      {
        _askConfirmationRestoreDocs = value;
      }
    }

    private bool _restoreOpenedDocumentsAfterDebug = true;
    public bool RestoreOpenedDocumentsAfterDebug
    {
      get
      {
        return _restoreOpenedDocumentsAfterDebug;
      }
      set
      {
        _restoreOpenedDocumentsAfterDebug = value;
      }
    }

    private bool _toolBarVisible = true;
    public bool ToolBarVisible
    {
      get
      {
        return _toolBarVisible;
      }
      set
      {
        _toolBarVisible = value;
      }
    }

    private int _toolBarRowIndex = 10;
    public int ToolBarRowIndex
    {
      get
      {
        return _toolBarRowIndex;
      }
      set
      {
        _toolBarRowIndex = value;
      }
    }

    public string ToolBarPosition { get; set; }

    private int _toolBarTop = -1;
    public int ToolBarTop
    {
      get
      {
        return _toolBarTop;
      }
      set
      {
        _toolBarTop = value;
      }
    }

    private int _toolBarLeft = -1;
    public int ToolBarLeft
    {
      get
      {
        return _toolBarLeft;
      }
      set
      {
        _toolBarLeft = value;
      }
    }

    //private int _toolBarWidth = -1;
    //public int ToolBarWidth
    //{
    //  get
    //  {
    //    return _toolBarWidth;
    //  }
    //  set
    //  {
    //    _toolBarWidth = value;
    //  }
    //}

    //private int _toolBarHeight = -1;
    //public int ToolBarHeight
    //{
    //  get
    //  {
    //    return _toolBarHeight;
    //  }
    //  set
    //  {
    //    _toolBarHeight = value;
    //  }
    //}
  }
}
