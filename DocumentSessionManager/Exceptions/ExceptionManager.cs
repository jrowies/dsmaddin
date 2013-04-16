using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocumentSessionManager
{
  public class ExceptionManager : IExceptionManager
  {
    public void HandleException(Exception ex)
    {
      MessageBox.Show(ex.ToString(), "Error in DocumentSessionManager addin", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
