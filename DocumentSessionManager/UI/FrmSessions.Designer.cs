namespace DocumentSessionManager
{
  partial class FrmSessions
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.buttonOk = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.panelBase = new System.Windows.Forms.Panel();
      this.labelName = new System.Windows.Forms.Label();
      this.labelExtra = new System.Windows.Forms.Label();
      this.textNewSessionName = new System.Windows.Forms.TextBox();
      this.panelGrid = new System.Windows.Forms.Panel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.sessionDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.dataGridDocuments = new System.Windows.Forms.DataGridView();
      this.Document = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.sessionDocumentDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.labelHeader = new System.Windows.Forms.Label();
      this.panelBase.SuspendLayout();
      this.panelGrid.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.sessionDtoBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridDocuments)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.sessionDocumentDtoBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOk
      // 
      this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOk.Location = new System.Drawing.Point(472, 386);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new System.Drawing.Size(75, 23);
      this.buttonOk.TabIndex = 0;
      this.buttonOk.Text = "&Ok";
      this.buttonOk.UseVisualStyleBackColor = true;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(564, 386);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 1;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.button2_Click);
      // 
      // panelBase
      // 
      this.panelBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelBase.Controls.Add(this.labelName);
      this.panelBase.Controls.Add(this.labelExtra);
      this.panelBase.Controls.Add(this.textNewSessionName);
      this.panelBase.Controls.Add(this.panelGrid);
      this.panelBase.Location = new System.Drawing.Point(0, 0);
      this.panelBase.Name = "panelBase";
      this.panelBase.Size = new System.Drawing.Size(651, 375);
      this.panelBase.TabIndex = 6;
      // 
      // labelName
      // 
      this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(12, 352);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(34, 13);
      this.labelName.TabIndex = 7;
      this.labelName.Text = "Name";
      this.labelName.Visible = false;
      // 
      // labelExtra
      // 
      this.labelExtra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelExtra.AutoSize = true;
      this.labelExtra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelExtra.Location = new System.Drawing.Point(12, 333);
      this.labelExtra.Name = "labelExtra";
      this.labelExtra.Size = new System.Drawing.Size(41, 13);
      this.labelExtra.TabIndex = 7;
      this.labelExtra.Text = "label1";
      this.labelExtra.Visible = false;
      // 
      // textNewSessionName
      // 
      this.textNewSessionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textNewSessionName.Location = new System.Drawing.Point(56, 349);
      this.textNewSessionName.Name = "textNewSessionName";
      this.textNewSessionName.Size = new System.Drawing.Size(583, 21);
      this.textNewSessionName.TabIndex = 0;
      this.textNewSessionName.Visible = false;
      // 
      // panelGrid
      // 
      this.panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelGrid.Controls.Add(this.splitContainer1);
      this.panelGrid.Controls.Add(this.labelHeader);
      this.panelGrid.Location = new System.Drawing.Point(0, 0);
      this.panelGrid.Name = "panelGrid";
      this.panelGrid.Size = new System.Drawing.Size(651, 321);
      this.panelGrid.TabIndex = 1;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer1.Location = new System.Drawing.Point(14, 25);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
      this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
      this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
      this.splitContainer1.Panel2.Controls.Add(this.dataGridDocuments);
      this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
      this.splitContainer1.Size = new System.Drawing.Size(624, 294);
      this.splitContainer1.SplitterDistance = 195;
      this.splitContainer1.TabIndex = 8;
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AutoGenerateColumns = false;
      this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
      this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.Column1});
      this.dataGridView1.DataSource = this.sessionDtoBindingSource;
      this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.dataGridView1.Location = new System.Drawing.Point(1, 0);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dataGridView1.RowTemplate.Height = 18;
      this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.ShowCellToolTips = false;
      this.dataGridView1.Size = new System.Drawing.Size(622, 194);
      this.dataGridView1.StandardTab = true;
      this.dataGridView1.TabIndex = 1;
      this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
      this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
      this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
      
      // 
      // nameDataGridViewTextBoxColumn
      // 
      this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
      this.nameDataGridViewTextBoxColumn.HeaderText = "Session Name";
      this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
      this.nameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // Column1
      // 
      this.Column1.DataPropertyName = "DocumentsCount";
      this.Column1.HeaderText = "Documents";
      this.Column1.Name = "Column1";
      this.Column1.ReadOnly = true;
      this.Column1.Width = 75;
      // 
      // sessionDtoBindingSource
      // 
      this.sessionDtoBindingSource.DataSource = typeof(DocumentSessionManager.SessionDto);
      // 
      // dataGridDocuments
      // 
      this.dataGridDocuments.AllowUserToAddRows = false;
      this.dataGridDocuments.AllowUserToDeleteRows = false;
      this.dataGridDocuments.AutoGenerateColumns = false;
      this.dataGridDocuments.BackgroundColor = System.Drawing.SystemColors.Window;
      this.dataGridDocuments.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dataGridDocuments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.dataGridDocuments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridDocuments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Document,
            this.pathDataGridViewTextBoxColumn});
      this.dataGridDocuments.DataSource = this.sessionDocumentDtoBindingSource;
      this.dataGridDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridDocuments.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.dataGridDocuments.Location = new System.Drawing.Point(1, 0);
      this.dataGridDocuments.Name = "dataGridDocuments";
      this.dataGridDocuments.ReadOnly = true;
      this.dataGridDocuments.RowHeadersVisible = false;
      this.dataGridDocuments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ButtonFace;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      this.dataGridDocuments.RowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridDocuments.RowTemplate.Height = 18;
      this.dataGridDocuments.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridDocuments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridDocuments.ShowCellToolTips = false;
      this.dataGridDocuments.Size = new System.Drawing.Size(622, 94);
      this.dataGridDocuments.StandardTab = true;
      this.dataGridDocuments.TabIndex = 8;
      this.dataGridDocuments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDocuments_CellDoubleClick);
      // 
      // Document
      // 
      this.Document.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Document.DataPropertyName = "Document";
      this.Document.FillWeight = 25F;
      this.Document.HeaderText = "Document";
      this.Document.Name = "Document";
      this.Document.ReadOnly = true;
      // 
      // pathDataGridViewTextBoxColumn
      // 
      this.pathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
      this.pathDataGridViewTextBoxColumn.FillWeight = 75F;
      this.pathDataGridViewTextBoxColumn.HeaderText = "Path";
      this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
      this.pathDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // sessionDocumentDtoBindingSource
      // 
      this.sessionDocumentDtoBindingSource.DataSource = typeof(DocumentSessionManager.SessionDocumentDto);
      // 
      // labelHeader
      // 
      this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelHeader.AutoSize = true;
      this.labelHeader.Location = new System.Drawing.Point(12, 9);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(35, 13);
      this.labelHeader.TabIndex = 6;
      this.labelHeader.Text = "label1";
      // 
      // FrmSessions
      // 
      this.AcceptButton = this.buttonOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(651, 421);
      this.Controls.Add(this.panelBase);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOk);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.KeyPreview = true;
      this.Name = "FrmSessions";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Document Session Manager";
      this.Shown += new System.EventHandler(this.FrmSessions_Shown);
      this.panelBase.ResumeLayout(false);
      this.panelBase.PerformLayout();
      this.panelGrid.ResumeLayout(false);
      this.panelGrid.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.sessionDtoBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridDocuments)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.sessionDocumentDtoBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Panel panelBase;
    private System.Windows.Forms.Label labelExtra;
    private System.Windows.Forms.TextBox textNewSessionName;
    private System.Windows.Forms.Panel panelGrid;
    private System.Windows.Forms.Label labelHeader;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    private System.Windows.Forms.DataGridView dataGridDocuments;
    private System.Windows.Forms.DataGridViewTextBoxColumn Document;
    private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
    private System.Windows.Forms.BindingSource sessionDtoBindingSource;
    private System.Windows.Forms.BindingSource sessionDocumentDtoBindingSource;
  }
}