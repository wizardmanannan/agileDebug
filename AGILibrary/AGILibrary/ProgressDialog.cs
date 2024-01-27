// Decompiled with JetBrains decompiler
// Type: AGI.ProgressDialog
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System.Drawing;
using System.Windows.Forms;

namespace AGI
{
  public class ProgressDialog : Form
  {
    protected bool abortCalled;
    protected int errorCount;
    private System.ComponentModel.Container components;
    private ProgressBar progressBarCurrent;
    private GroupBox groupBoxCurrent;
    private Label labelCurrent;
    private GroupBox groupBoxOutput;
    private TextBox textBoxOutput;
    private Button buttonClose;

    public ProgressDialog(string titleBar)
    {
      this.InitializeComponent();
      if (titleBar != "")
        this.Text = titleBar;
      this.progressBarCurrent.Hide();
      this.abortCalled = false;
      this.errorCount = 0;
    }

    public string CurrentInfo
    {
      set
      {
        this.labelCurrent.Text = value;
        this.Update();
        this.Validate();
      }
    }

    public int MaxProgress
    {
      get => this.progressBarCurrent.Maximum;
      set
      {
        this.progressBarCurrent.Minimum = 0;
        this.progressBarCurrent.Maximum = value;
        this.progressBarCurrent.Step = 1;
        this.progressBarCurrent.Show();
      }
    }

    public void AddInfo(string text)
    {
      this.textBoxOutput.AppendText(text + "\r\n");
      this.Update();
    }

    public void AddError(string text)
    {
      this.textBoxOutput.AppendText(text + "\r\n");
      ++this.errorCount;
      this.Update();
    }

    public bool Step()
    {
      this.progressBarCurrent.PerformStep();
      this.Update();
      return this.abortCalled;
    }

    public void Begin()
    {
      this.Show();
      this.Update();
    }

    public void EndFail()
    {
      this.CurrentInfo = "Failed!";
      this.AddInfo("");
      this.AddInfo("The operation failed.");
      this.buttonClose.Text = "OK";
      this.buttonClose.Enabled = true;
      this.Hide();
      int num = (int) this.ShowDialog();
    }

    public void EndSuccess()
    {
      if (this.errorCount > 0)
      {
        this.CurrentInfo = "Completed with errors";
        this.AddInfo(string.Format("The operation completed with {0} errors.", (object) this.errorCount));
        this.buttonClose.Text = "Finish";
        this.buttonClose.Enabled = true;
        this.Hide();
        int num = (int) this.ShowDialog();
      }
      else
      {
        this.CurrentInfo = "Completed";
        this.AddInfo("The operation completed successfully.");
        this.buttonClose.Text = "Finish";
        this.buttonClose.Enabled = true;
        this.Close();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.progressBarCurrent = new ProgressBar();
      this.groupBoxCurrent = new GroupBox();
      this.labelCurrent = new Label();
      this.groupBoxOutput = new GroupBox();
      this.textBoxOutput = new TextBox();
      this.buttonClose = new Button();
      this.groupBoxCurrent.SuspendLayout();
      this.groupBoxOutput.SuspendLayout();
      this.SuspendLayout();
      this.progressBarCurrent.Dock = DockStyle.Bottom;
      this.progressBarCurrent.Location = new Point(3, 38);
      this.progressBarCurrent.Name = "progressBarCurrent";
      this.progressBarCurrent.Size = new Size(306, 23);
      this.progressBarCurrent.TabIndex = 0;
      this.groupBoxCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxCurrent.Controls.Add((Control) this.labelCurrent);
      this.groupBoxCurrent.Controls.Add((Control) this.progressBarCurrent);
      this.groupBoxCurrent.Location = new Point(8, 8);
      this.groupBoxCurrent.Name = "groupBoxCurrent";
      this.groupBoxCurrent.Size = new Size(312, 64);
      this.groupBoxCurrent.TabIndex = 1;
      this.groupBoxCurrent.TabStop = false;
      this.groupBoxCurrent.Text = "Current Progress";
      this.labelCurrent.Dock = DockStyle.Fill;
      this.labelCurrent.Location = new Point(3, 16);
      this.labelCurrent.Name = "labelCurrent";
      this.labelCurrent.Size = new Size(306, 22);
      this.labelCurrent.TabIndex = 1;
      this.labelCurrent.TextAlign = ContentAlignment.MiddleLeft;
      this.groupBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxOutput.Controls.Add((Control) this.textBoxOutput);
      this.groupBoxOutput.Location = new Point(8, 80);
      this.groupBoxOutput.Name = "groupBoxOutput";
      this.groupBoxOutput.Size = new Size(312, 208);
      this.groupBoxOutput.TabIndex = 2;
      this.groupBoxOutput.TabStop = false;
      this.groupBoxOutput.Text = "Output";
      this.textBoxOutput.Dock = DockStyle.Fill;
      this.textBoxOutput.Location = new Point(3, 16);
      this.textBoxOutput.Multiline = true;
      this.textBoxOutput.Name = "textBoxOutput";
      this.textBoxOutput.ReadOnly = true;
      this.textBoxOutput.ScrollBars = ScrollBars.Both;
      this.textBoxOutput.Size = new Size(306, 189);
      this.textBoxOutput.TabIndex = 0;
      this.textBoxOutput.WordWrap = false;
      this.buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClose.DialogResult = DialogResult.OK;
      this.buttonClose.Enabled = false;
      this.buttonClose.Location = new Point((int) sbyte.MaxValue, 304);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new Size(75, 23);
      this.buttonClose.TabIndex = 3;
      this.buttonClose.Text = "Close";
      this.AcceptButton = (IButtonControl) this.buttonClose;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(328, 334);
      this.ControlBox = false;
      this.Controls.Add((Control) this.buttonClose);
      this.Controls.Add((Control) this.groupBoxOutput);
      this.Controls.Add((Control) this.groupBoxCurrent);
      this.Name = nameof (ProgressDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Progress Dialog";
      this.groupBoxCurrent.ResumeLayout(false);
      this.groupBoxOutput.ResumeLayout(false);
      this.groupBoxOutput.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
