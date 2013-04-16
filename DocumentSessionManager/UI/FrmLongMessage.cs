using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocumentSessionManager
{
  public partial class FrmLongMessage : Form
  {
    private string _caption;
    private string _message;
    private string _longMessage;

    public FrmLongMessage(string caption, string message, string longMessage) : base()
    {
      InitializeComponent();

      _longMessage = longMessage;
      _message = message;
      _caption = caption;

      ShowInTaskbar = false;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void FrmLongMessage_Shown(object sender, EventArgs e)
    {
      Text = _caption;
      labelMessage.Text = _message;
      textMessage.Text = _longMessage;
    }
  }
}
