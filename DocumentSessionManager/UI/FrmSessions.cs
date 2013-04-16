using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EnvDTE80;

namespace DocumentSessionManager
{
  public partial class FrmSessions : Form
  {
    private const string STR_SaveAs_OkButton = "&Save";

    private IDsmSettingsManager _settings;

    private IDteAdapter _dteAdapter;

    private IEnumerable<SessionDto> _currentSessions;

    private bool _saveAsMode;

    private IExceptionManager _exceptionManager;

    private FrmSessions()
    {
    }

    public FrmSessions(IDteAdapter dteAdapter, IDsmSettingsManager settings, IExceptionManager exceptionManager)
      : base()
    {
      InitializeComponent();

      splitContainer1.Panel2Collapsed = true;

      _dteAdapter = dteAdapter;
      _settings = settings;
      _exceptionManager = exceptionManager;

      ShowInTaskbar = false;
    }

    private IList<SessionDto> _result = new List<SessionDto>();

    public IList<SessionDto> Result
    {
      get
      {
        return _result;
      }
    }

    public void ConfigureForRecentlyClosedDocuments(IEnumerable<SessionDocumentDto> recentlyClosedDocuments)
    {
      fillGridWithDocuments(recentlyClosedDocuments);

      this.Text = "Recently closed documents";
      labelHeader.Text = "Double-click a document to open";
      buttonOk.Visible = false;
      buttonCancel.Text = "&Close";
      panelGrid.Dock = DockStyle.Fill;

      splitContainer1.Panel1Collapsed = true;
    }

    public void ConfigureForLoading(IEnumerable<SessionDto> currentSessions)
    {
      dataGridView1.DataSource = currentSessions;

      dataGridView1.MultiSelect = false;
      labelHeader.Text = "Select the session to load";
      buttonOk.Text = "&Load";
      panelGrid.Dock = DockStyle.Fill;

      buttonOk.Click += (sender, e) =>
      {
        try
        {
          if (dataGridView1.CurrentRow != null)
          {
            SessionDto s = dataGridView1.CurrentRow.DataBoundItem as SessionDto;
            if (!_settings.DsmSettings.AskConfirmationOnLoad || AskForConfirmation("Session '{0}' will be loaded, do you want to continue?", s.Name))
            {
              _result.Add(s);
              Close();
            }
          }
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
        }
      };
    }


    public void ConfigureForDelete(IEnumerable<SessionDto> currentSessions)
    {
      dataGridView1.DataSource = currentSessions;

      dataGridView1.MultiSelect = true;
      labelHeader.Text = "Select one or more sessions to delete";
      buttonOk.Text = "&Delete";
      panelGrid.Dock = DockStyle.Fill;

      buttonOk.Click += (sender, e) =>
      {
        try
        {
          if (dataGridView1.SelectedRows.Count > 0)
          {
            if (!_settings.DsmSettings.AskConfirmationOnDelete || AskForConfirmation("{0} sessions will be deleted, do you want to continue?", dataGridView1.SelectedRows.Count))
            {
              foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                _result.Add(row.DataBoundItem as SessionDto);

              Close();
            }
          }
          else if (dataGridView1.CurrentRow != null)
          {
            if (!_settings.DsmSettings.AskConfirmationOnDelete || AskForConfirmation("{0} sessions will be deleted, do you want to continue?", 1))
            {
              _result.Add(dataGridView1.CurrentRow.DataBoundItem as SessionDto);
              Close();
            }
          }
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
        }
      };

    }

    public void ConfigureForSaveAs(IEnumerable<SessionDto> currentSessions)
    {
      dataGridView1.DataSource = currentSessions;
      _currentSessions = currentSessions;

      dataGridView1.MultiSelect = false;
      dataGridView1.RowEnter += rowEnter;

      labelExtra.Text = "Save the current session as:";

      textNewSessionName.TextChanged += textChanged;
      labelHeader.Text = "Double-click an existing session to replace it";

      buttonOk.Text = STR_SaveAs_OkButton;

      textNewSessionName.Visible = true;
      labelExtra.Visible = true;
      labelName.Visible = true;

      _saveAsMode = true;

      buttonOk.Click += (sender, e) =>
      {
        try
        {
          var session = _currentSessions.FirstOrDefault(s => s.Name.Equals(textNewSessionName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
          if (session != null)
          {
            if (!_settings.DsmSettings.AskConfirmationOnSaveAs || AskForConfirmation("Session '{0}' will be replaced, do you want to continue?", session.Name))
            {
              _result.Add(session);
              Close();
            }
          }
          else
          {
            var newSession = new SessionDto { Name = textNewSessionName.Text.Trim() };
            if (!_settings.DsmSettings.AskConfirmationOnSaveAs || AskForConfirmation("Session '{0}' will be saved, do you want to continue?", newSession.Name))
            {
              _result.Add(newSession);
              Close();
            }
          }
        }
        catch (Exception ex)
        {
          _exceptionManager.HandleException(ex);
        }
      };

    }

    public static bool AskForConfirmation(string confirmationMsg, params object[] args)
    {
      DialogResult result;

      if (args.Length > 0)
        result = MessageBox.Show(string.Format(confirmationMsg, args), "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
      else
        result = MessageBox.Show(confirmationMsg, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

      return result == DialogResult.OK;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void rowEnter(object sender, DataGridViewCellEventArgs e)
    {
      var s = dataGridView1.Rows[e.RowIndex].DataBoundItem as SessionDto;
      if (s != null)
      {
        textNewSessionName.Text = s.Name;
      }
    }

    private void textChanged(object sender, EventArgs e)
    {
      if (_currentSessions.FirstOrDefault(s => s.Name.Equals(textNewSessionName.Text.Trim(), StringComparison.OrdinalIgnoreCase)) != null)
        buttonOk.Text = "&Replace";
      else
        buttonOk.Text = STR_SaveAs_OkButton;

      buttonOk.Enabled = !string.IsNullOrEmpty(textNewSessionName.Text.Trim());
    }

    private void dataGridView1_DoubleClick(object sender, EventArgs e)
    {
      buttonOk.PerformClick();
    }

    private void FrmSessions_Shown(object sender, EventArgs e)
    {
      if (_saveAsMode)
      {
        textNewSessionName.Text = "New Session";
        textNewSessionName.Select();
      }
      else
        dataGridView1.Select();
    }

    private void fillGridWithDocuments(IEnumerable<SessionDocumentDto> documents)
    {
      dataGridDocuments.DataSource = documents;
    }

    private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
      var s = dataGridView1.Rows[e.RowIndex].DataBoundItem as SessionDto;
      if (s != null)
      {
        if (splitContainer1.Panel2Collapsed)
          splitContainer1.Panel2Collapsed = false;

        fillGridWithDocuments(s.Documents);
      }
    }

    private void dataGridDocuments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      var di = dataGridDocuments.Rows[e.RowIndex].DataBoundItem as SessionDocumentDto;
      if (di != null)
      {
        _dteAdapter.OpenFile(di.Path, di.Type);
      }
    }

    private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.Handled = true;
        buttonOk.PerformClick();
      }
    }
  }

}
