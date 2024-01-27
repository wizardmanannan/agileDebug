// Decompiled with JetBrains decompiler
// Type: AGI.PicPalette
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AGI
{
  public class PicPalette : UserControl
  {
    private static Color selectedColor = Color.Black;
    public static int colIdx = (int) PicPalette.selectedColor.ColorIndex;
    private Panel switchPanel;
    public RadioButton visualRB;
    public RadioButton priorityRB;
    public RadioButton offRB;
    private TextBox colorNameTxtBx;
    private RotatedLabel offLabel;
    private RotatedLabel visualLabel;
    private RotatedLabel priorityLabel;
    private ToolTip toolTip;
    private IContainer components;
    private PictureBox greenBox;
    private PictureBox blackBox;
    private PictureBox blueBox;
    private PictureBox cyanBox;
    private PictureBox redBox;
    private PictureBox magentaBox;
    private PictureBox brownBox;
    private PictureBox brightGrayBox;
    private PictureBox grayBox;
    private PictureBox brightBlueBox;
    private PictureBox brightGreenBox;
    private PictureBox brightCyanBox;
    private PictureBox brightRedBox;
    private PictureBox brightMagentaBox;
    private PictureBox whiteBox;
    private PictureBox yellowBox;
    private PictureBox selectedColorBox;

    public PicPalette() => this.InitializeComponent();

    private void offRB_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.offRB.Checked)
        return;
      this.Invalidate();
    }

    private void offLabel_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
        this.colorNameTxtBx.Text = "No Color Selected";
      }
      else
        this.colorNameTxtBx.ForeColor = SystemColors.WindowText;
      this.offRB.Checked = true;
    }

    private void visualLabel_Click(object sender, EventArgs e) => this.visualRB.Checked = true;

    private void priorityLabel_Click(object sender, EventArgs e) => this.priorityRB.Checked = true;

    private void blackBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        this.SelColor = (byte) 0;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Black";
        this.selectedColorBox.BackColor = System.Drawing.Color.Black;
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Black");
      }
    }

    private void blueBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        this.SelColor = (byte) 1;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Blue";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(0, 0, 170);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Blue");
      }
    }

    private void greenBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.Green;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Green";
        this.selectedColorBox.BackColor = System.Drawing.Color.Green;
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Green");
      }
    }

    private void cyanBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.Cyan;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Cyan";
        this.selectedColorBox.BackColor = System.Drawing.Color.Teal;
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Cyan");
      }
    }

    private void redBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.Red;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Red";
        this.selectedColorBox.BackColor = System.Drawing.Color.Red;
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Red");
      }
    }

    private void magentaBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.Magenta;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Magenta";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(170, 0, 170);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Magenta");
      }
    }

    private void brownBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.Brown;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Brown";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(170, 85, 0);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Brown");
      }
    }

    private void brightGrayBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.BrightGray;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Bright Gray";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(170, 170, 170);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Bright Gray");
      }
    }

    private void grayBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.Gray;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Gray";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(85, 85, 85);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Gray");
      }
    }

    private void brightBlueBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.BrightBlue;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Bright Blue";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(85, 85, (int) byte.MaxValue);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Bright Blue");
      }
    }

    private void brightGreenBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.BrightGreen;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Bright Green";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(85, (int) byte.MaxValue, 85);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Bright Green");
      }
    }

    private void brightCyanBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.BrightCyan;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Bright Cyan";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb(85, (int) byte.MaxValue, (int) byte.MaxValue);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Bright Cyan");
      }
    }

    private void brightRedBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.BrightRed;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Bright Red";
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 85, 85);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Bright Red");
      }
    }

    private void brightMagentaBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.BrightMagenta;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "Bright Magenta";
        this.colorNameTxtBx.ForeColor = SystemColors.WindowText;
        this.selectedColorBox.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 85, (int) byte.MaxValue);
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "Bright Magenta");
      }
    }

    private void yellowBox_Click(object sender, EventArgs e)
    {
      this.SelColor = (byte) 14;
      PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
      this.colorNameTxtBx.Text = PicPalette.GetName();
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        this.SelColor = (byte) 14;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = PicPalette.GetName();
        this.PicPalette_Click((object) null, (EventArgs) null);
        PicPalette.selectedColor = (Color) PicPalette.selectedColor.ColorIndex;
      }
    }

    private void whiteBox_Click(object sender, EventArgs e)
    {
      if (this.offRB.Checked)
      {
        PicPalette.selectedColor = (Color) null;
        this.selectedColorBox.BackColor = SystemColors.Control;
        this.colorNameTxtBx.ForeColor = SystemColors.ControlDark;
      }
      else
      {
        PicPalette.selectedColor = Color.White;
        PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;
        this.colorNameTxtBx.Text = "White";
        this.selectedColorBox.BackColor = System.Drawing.Color.White;
        this.toolTip.SetToolTip((Control) this.selectedColorBox, "White");
      }
    }

    [Category("Appearance")]
    [Description("The color selected for left mouse button")]
    [DefaultValue(15)]
    public byte SelColor
    {
      get => (byte) PicPalette.selectedColor;
      set
      {
        PicPalette.selectedColor = (Color) value;
        this.Invalidate();
      }
    }

    public static string GetName()
    {
      string name = (string) null;
      if (PicPalette.selectedColor == Color.Black)
        name = "";
      if (PicPalette.selectedColor == Color.Black)
        name = "Black";
      if (PicPalette.selectedColor == Color.Blue)
        name = "Blue";
      if (PicPalette.selectedColor == Color.Green)
        name = "Green";
      if (PicPalette.selectedColor == Color.Cyan)
        name = "Cyan";
      if (PicPalette.selectedColor == Color.Red)
        name = "Red";
      if (PicPalette.selectedColor == Color.Magenta)
        name = "Magenta";
      if (PicPalette.selectedColor == Color.Brown)
        name = "Brown";
      if (PicPalette.selectedColor == Color.BrightGray)
        name = " Bright Gray";
      if (PicPalette.selectedColor == Color.Gray)
        name = "Gray";
      if (PicPalette.selectedColor == Color.BrightBlue)
        name = " Bright Blue";
      if (PicPalette.selectedColor == Color.BrightGreen)
        name = " Bright Green";
      if (PicPalette.selectedColor == Color.BrightCyan)
        name = " Bright Cyan";
      if (PicPalette.selectedColor == Color.BrightRed)
        name = " Bright Red ";
      if (PicPalette.selectedColor == Color.BrightMagenta)
        name = " Bright Magenta";
      if (PicPalette.selectedColor == Color.Yellow)
        name = "Yellow";
      if (PicPalette.selectedColor == Color.White)
        name = "White";
      return name;
    }

    private void yellowBox_MouseDown(object sender, MouseEventArgs e)
    {
    }

    private void yellowBox_MouseClick(object sender, MouseEventArgs e)
    {
    }

    private void PicPalette_Click(object sender, EventArgs e) => PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;

    private void PicPalette_MouseDown(object sender, MouseEventArgs e) => PicPalette.colIdx = (int) PicPalette.selectedColor.ColorIndex;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.switchPanel = new Panel();
      this.offRB = new RadioButton();
      this.visualRB = new RadioButton();
      this.priorityRB = new RadioButton();
      this.colorNameTxtBx = new TextBox();
      this.toolTip = new ToolTip(this.components);
      this.blackBox = new PictureBox();
      this.blueBox = new PictureBox();
      this.greenBox = new PictureBox();
      this.cyanBox = new PictureBox();
      this.redBox = new PictureBox();
      this.magentaBox = new PictureBox();
      this.brownBox = new PictureBox();
      this.brightGrayBox = new PictureBox();
      this.grayBox = new PictureBox();
      this.brightBlueBox = new PictureBox();
      this.brightGreenBox = new PictureBox();
      this.brightCyanBox = new PictureBox();
      this.brightRedBox = new PictureBox();
      this.brightMagentaBox = new PictureBox();
      this.whiteBox = new PictureBox();
      this.yellowBox = new PictureBox();
      this.selectedColorBox = new PictureBox();
      this.offLabel = new RotatedLabel();
      this.visualLabel = new RotatedLabel();
      this.priorityLabel = new RotatedLabel();
      this.switchPanel.SuspendLayout();
      ((ISupportInitialize) this.blackBox).BeginInit();
      ((ISupportInitialize) this.blueBox).BeginInit();
      ((ISupportInitialize) this.greenBox).BeginInit();
      ((ISupportInitialize) this.cyanBox).BeginInit();
      ((ISupportInitialize) this.redBox).BeginInit();
      ((ISupportInitialize) this.magentaBox).BeginInit();
      ((ISupportInitialize) this.brownBox).BeginInit();
      ((ISupportInitialize) this.brightGrayBox).BeginInit();
      ((ISupportInitialize) this.grayBox).BeginInit();
      ((ISupportInitialize) this.brightBlueBox).BeginInit();
      ((ISupportInitialize) this.brightGreenBox).BeginInit();
      ((ISupportInitialize) this.brightCyanBox).BeginInit();
      ((ISupportInitialize) this.brightRedBox).BeginInit();
      ((ISupportInitialize) this.brightMagentaBox).BeginInit();
      ((ISupportInitialize) this.whiteBox).BeginInit();
      ((ISupportInitialize) this.yellowBox).BeginInit();
      ((ISupportInitialize) this.selectedColorBox).BeginInit();
      this.SuspendLayout();
      this.switchPanel.Controls.Add((Control) this.offRB);
      this.switchPanel.Controls.Add((Control) this.visualRB);
      this.switchPanel.Controls.Add((Control) this.priorityRB);
      this.switchPanel.Location = new Point(5, 230);
      this.switchPanel.Name = "switchPanel";
      this.switchPanel.Size = new Size(43, 18);
      this.switchPanel.TabIndex = 0;
      this.offRB.AutoSize = true;
      this.offRB.Checked = true;
      this.offRB.Location = new Point(1, 3);
      this.offRB.Name = "offRB";
      this.offRB.Size = new Size(14, 13);
      this.offRB.TabIndex = 1;
      this.offRB.TabStop = true;
      this.offRB.TextAlign = ContentAlignment.BottomCenter;
      this.offRB.TextImageRelation = TextImageRelation.ImageAboveText;
      this.offRB.UseVisualStyleBackColor = true;
      this.offRB.CheckedChanged += new EventHandler(this.offRB_CheckedChanged);
      this.visualRB.AutoSize = true;
      this.visualRB.Location = new Point(15, 3);
      this.visualRB.Name = "visualRB";
      this.visualRB.Size = new Size(14, 13);
      this.visualRB.TabIndex = 0;
      this.visualRB.UseVisualStyleBackColor = true;
      this.priorityRB.AutoSize = true;
      this.priorityRB.Location = new Point(29, 3);
      this.priorityRB.Name = "priorityRB";
      this.priorityRB.Size = new Size(14, 13);
      this.priorityRB.TabIndex = 0;
      this.priorityRB.UseVisualStyleBackColor = true;
      this.colorNameTxtBx.Anchor = AnchorStyles.Top;
      this.colorNameTxtBx.BackColor = SystemColors.Control;
      this.colorNameTxtBx.BorderStyle = BorderStyle.None;
      this.colorNameTxtBx.Location = new Point(7, 4);
      this.colorNameTxtBx.Multiline = true;
      this.colorNameTxtBx.Name = "colorNameTxtBx";
      this.colorNameTxtBx.ReadOnly = true;
      this.colorNameTxtBx.Size = new Size(43, 28);
      this.colorNameTxtBx.TabIndex = 3;
      this.colorNameTxtBx.Text = "No Color Selected";
      this.colorNameTxtBx.TextAlign = HorizontalAlignment.Center;
      this.blackBox.BackColor = System.Drawing.Color.Black;
      this.blackBox.BorderStyle = BorderStyle.FixedSingle;
      this.blackBox.Location = new Point(8, 72);
      this.blackBox.Name = "blackBox";
      this.blackBox.Size = new Size(16, 16);
      this.blackBox.TabIndex = 5;
      this.blackBox.TabStop = false;
      this.blackBox.Tag = (object) "Black";
      this.toolTip.SetToolTip((Control) this.blackBox, "Black");
      this.blackBox.Click += new EventHandler(this.blackBox_Click);
      this.blueBox.BackColor = System.Drawing.Color.FromArgb(0, 0, 170);
      this.blueBox.BorderStyle = BorderStyle.FixedSingle;
      this.blueBox.Location = new Point(28, 72);
      this.blueBox.Name = "blueBox";
      this.blueBox.Size = new Size(17, 16);
      this.blueBox.TabIndex = 6;
      this.blueBox.TabStop = false;
      this.blueBox.Tag = (object) "Blue";
      this.toolTip.SetToolTip((Control) this.blueBox, "Blue");
      this.blueBox.Click += new EventHandler(this.blueBox_Click);
      this.greenBox.BackColor = System.Drawing.Color.Green;
      this.greenBox.BorderStyle = BorderStyle.FixedSingle;
      this.greenBox.Location = new Point(8, 92);
      this.greenBox.Name = "greenBox";
      this.greenBox.Size = new Size(16, 16);
      this.greenBox.TabIndex = 4;
      this.greenBox.TabStop = false;
      this.greenBox.Tag = (object) "Green";
      this.toolTip.SetToolTip((Control) this.greenBox, "Green");
      this.greenBox.Click += new EventHandler(this.greenBox_Click);
      this.cyanBox.BackColor = System.Drawing.Color.Teal;
      this.cyanBox.BorderStyle = BorderStyle.FixedSingle;
      this.cyanBox.Location = new Point(28, 92);
      this.cyanBox.Name = "cyanBox";
      this.cyanBox.Size = new Size(16, 16);
      this.cyanBox.TabIndex = 7;
      this.cyanBox.TabStop = false;
      this.cyanBox.Tag = (object) "Cyan";
      this.toolTip.SetToolTip((Control) this.cyanBox, "Cyan");
      this.cyanBox.Click += new EventHandler(this.cyanBox_Click);
      this.redBox.BackColor = System.Drawing.Color.Red;
      this.redBox.BorderStyle = BorderStyle.FixedSingle;
      this.redBox.Location = new Point(8, 112);
      this.redBox.Name = "redBox";
      this.redBox.Size = new Size(16, 16);
      this.redBox.TabIndex = 8;
      this.redBox.TabStop = false;
      this.redBox.Tag = (object) "Red";
      this.toolTip.SetToolTip((Control) this.redBox, "Red");
      this.redBox.Click += new EventHandler(this.redBox_Click);
      this.magentaBox.BackColor = System.Drawing.Color.FromArgb(170, 0, 170);
      this.magentaBox.BorderStyle = BorderStyle.FixedSingle;
      this.magentaBox.Location = new Point(28, 112);
      this.magentaBox.Name = "magentaBox";
      this.magentaBox.Size = new Size(16, 16);
      this.magentaBox.TabIndex = 9;
      this.magentaBox.TabStop = false;
      this.magentaBox.Tag = (object) "Magenta";
      this.toolTip.SetToolTip((Control) this.magentaBox, "Magenta");
      this.magentaBox.Click += new EventHandler(this.magentaBox_Click);
      this.brownBox.BackColor = System.Drawing.Color.FromArgb(170, 85, 0);
      this.brownBox.BorderStyle = BorderStyle.FixedSingle;
      this.brownBox.Location = new Point(8, 132);
      this.brownBox.Name = "brownBox";
      this.brownBox.Size = new Size(16, 16);
      this.brownBox.TabIndex = 10;
      this.brownBox.TabStop = false;
      this.brownBox.Tag = (object) "Brown";
      this.toolTip.SetToolTip((Control) this.brownBox, "Brown");
      this.brownBox.Click += new EventHandler(this.brownBox_Click);
      this.brightGrayBox.BackColor = System.Drawing.Color.FromArgb(170, 170, 170);
      this.brightGrayBox.BorderStyle = BorderStyle.FixedSingle;
      this.brightGrayBox.Location = new Point(28, 132);
      this.brightGrayBox.Name = "brightGrayBox";
      this.brightGrayBox.Size = new Size(16, 16);
      this.brightGrayBox.TabIndex = 11;
      this.brightGrayBox.TabStop = false;
      this.brightGrayBox.Tag = (object) "BrightGray";
      this.toolTip.SetToolTip((Control) this.brightGrayBox, "Bright Gray");
      this.brightGrayBox.Click += new EventHandler(this.brightGrayBox_Click);
      this.grayBox.BackColor = System.Drawing.Color.FromArgb(85, 85, 85);
      this.grayBox.BorderStyle = BorderStyle.FixedSingle;
      this.grayBox.Location = new Point(8, 152);
      this.grayBox.Name = "grayBox";
      this.grayBox.Size = new Size(16, 16);
      this.grayBox.TabIndex = 12;
      this.grayBox.TabStop = false;
      this.grayBox.Tag = (object) "Gray";
      this.toolTip.SetToolTip((Control) this.grayBox, "Gray");
      this.grayBox.Click += new EventHandler(this.grayBox_Click);
      this.brightBlueBox.BackColor = System.Drawing.Color.FromArgb(85, 85, (int) byte.MaxValue);
      this.brightBlueBox.BorderStyle = BorderStyle.FixedSingle;
      this.brightBlueBox.Location = new Point(28, 152);
      this.brightBlueBox.Name = "brightBlueBox";
      this.brightBlueBox.Size = new Size(16, 16);
      this.brightBlueBox.TabIndex = 13;
      this.brightBlueBox.TabStop = false;
      this.brightBlueBox.Tag = (object) "BrightBlue";
      this.toolTip.SetToolTip((Control) this.brightBlueBox, "Bright Blue");
      this.brightBlueBox.Click += new EventHandler(this.brightBlueBox_Click);
      this.brightGreenBox.BackColor = System.Drawing.Color.FromArgb(85, (int) byte.MaxValue, 85);
      this.brightGreenBox.BorderStyle = BorderStyle.FixedSingle;
      this.brightGreenBox.Location = new Point(8, 172);
      this.brightGreenBox.Name = "brightGreenBox";
      this.brightGreenBox.Size = new Size(16, 16);
      this.brightGreenBox.TabIndex = 14;
      this.brightGreenBox.TabStop = false;
      this.brightGreenBox.Tag = (object) "BrightGreen";
      this.toolTip.SetToolTip((Control) this.brightGreenBox, "Bright Green");
      this.brightGreenBox.Click += new EventHandler(this.brightGreenBox_Click);
      this.brightCyanBox.BackColor = System.Drawing.Color.FromArgb(85, (int) byte.MaxValue, (int) byte.MaxValue);
      this.brightCyanBox.BorderStyle = BorderStyle.FixedSingle;
      this.brightCyanBox.Location = new Point(28, 172);
      this.brightCyanBox.Name = "brightCyanBox";
      this.brightCyanBox.Size = new Size(16, 16);
      this.brightCyanBox.TabIndex = 15;
      this.brightCyanBox.TabStop = false;
      this.brightCyanBox.Tag = (object) "BrightCyan";
      this.toolTip.SetToolTip((Control) this.brightCyanBox, "Bright Cyan");
      this.brightCyanBox.Click += new EventHandler(this.brightCyanBox_Click);
      this.brightRedBox.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 85, 85);
      this.brightRedBox.BorderStyle = BorderStyle.FixedSingle;
      this.brightRedBox.Location = new Point(8, 192);
      this.brightRedBox.Name = "brightRedBox";
      this.brightRedBox.Size = new Size(16, 16);
      this.brightRedBox.TabIndex = 16;
      this.brightRedBox.TabStop = false;
      this.brightRedBox.Tag = (object) "BrightRed";
      this.toolTip.SetToolTip((Control) this.brightRedBox, "Bright Red");
      this.brightRedBox.Click += new EventHandler(this.brightRedBox_Click);
      this.brightMagentaBox.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 85, (int) byte.MaxValue);
      this.brightMagentaBox.BorderStyle = BorderStyle.FixedSingle;
      this.brightMagentaBox.Location = new Point(28, 192);
      this.brightMagentaBox.Name = "brightMagentaBox";
      this.brightMagentaBox.Size = new Size(16, 16);
      this.brightMagentaBox.TabIndex = 17;
      this.brightMagentaBox.TabStop = false;
      this.brightMagentaBox.Tag = (object) "BrightMagenta";
      this.toolTip.SetToolTip((Control) this.brightMagentaBox, "Bright Magenta");
      this.brightMagentaBox.Click += new EventHandler(this.brightMagentaBox_Click);
      this.whiteBox.BackColor = System.Drawing.Color.White;
      this.whiteBox.BorderStyle = BorderStyle.FixedSingle;
      this.whiteBox.Location = new Point(28, 212);
      this.whiteBox.Name = "whiteBox";
      this.whiteBox.Size = new Size(16, 16);
      this.whiteBox.TabIndex = 19;
      this.whiteBox.TabStop = false;
      this.whiteBox.Tag = (object) "White";
      this.toolTip.SetToolTip((Control) this.whiteBox, "White");
      this.whiteBox.Click += new EventHandler(this.whiteBox_Click);
      this.yellowBox.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 85);
      this.yellowBox.BorderStyle = BorderStyle.FixedSingle;
      this.yellowBox.Location = new Point(8, 212);
      this.yellowBox.Name = "yellowBox";
      this.yellowBox.Size = new Size(16, 16);
      this.yellowBox.TabIndex = 18;
      this.yellowBox.TabStop = false;
      this.yellowBox.Tag = (object) "Yellow";
      this.toolTip.SetToolTip((Control) this.yellowBox, "Yellow");
      this.yellowBox.Click += new EventHandler(this.yellowBox_Click);
      this.yellowBox.MouseClick += new MouseEventHandler(this.yellowBox_MouseClick);
      this.yellowBox.MouseDown += new MouseEventHandler(this.yellowBox_MouseDown);
      this.selectedColorBox.BackColor = System.Drawing.Color.Transparent;
      this.selectedColorBox.BorderStyle = BorderStyle.FixedSingle;
      this.selectedColorBox.Location = new Point(8, 32);
      this.selectedColorBox.Name = "selectedColorBox";
      this.selectedColorBox.Size = new Size(38, 36);
      this.selectedColorBox.TabIndex = 20;
      this.selectedColorBox.TabStop = false;
      this.offLabel.Location = new Point(3, 248);
      this.offLabel.Name = "offLabel";
      this.offLabel.NewText = "Off";
      this.offLabel.RotateAngle = 90;
      this.offLabel.Size = new Size(15, 20);
      this.offLabel.TabIndex = 0;
      this.offLabel.Click += new EventHandler(this.offLabel_Click);
      this.visualLabel.Location = new Point(17, 248);
      this.visualLabel.Name = "visualLabel";
      this.visualLabel.NewText = "Visual";
      this.visualLabel.RotateAngle = 90;
      this.visualLabel.Size = new Size(15, 36);
      this.visualLabel.TabIndex = 1;
      this.visualLabel.Click += new EventHandler(this.visualLabel_Click);
      this.priorityLabel.Location = new Point(31, 248);
      this.priorityLabel.Name = "priorityLabel";
      this.priorityLabel.NewText = "Priority";
      this.priorityLabel.RotateAngle = 90;
      this.priorityLabel.Size = new Size(15, 41);
      this.priorityLabel.TabIndex = 2;
      this.priorityLabel.Click += new EventHandler(this.priorityLabel_Click);
      this.Controls.Add((Control) this.selectedColorBox);
      this.Controls.Add((Control) this.whiteBox);
      this.Controls.Add((Control) this.yellowBox);
      this.Controls.Add((Control) this.brightMagentaBox);
      this.Controls.Add((Control) this.brightRedBox);
      this.Controls.Add((Control) this.brightCyanBox);
      this.Controls.Add((Control) this.brightGreenBox);
      this.Controls.Add((Control) this.brightBlueBox);
      this.Controls.Add((Control) this.grayBox);
      this.Controls.Add((Control) this.brightGrayBox);
      this.Controls.Add((Control) this.brownBox);
      this.Controls.Add((Control) this.magentaBox);
      this.Controls.Add((Control) this.redBox);
      this.Controls.Add((Control) this.cyanBox);
      this.Controls.Add((Control) this.blueBox);
      this.Controls.Add((Control) this.blackBox);
      this.Controls.Add((Control) this.greenBox);
      this.Controls.Add((Control) this.offLabel);
      this.Controls.Add((Control) this.visualLabel);
      this.Controls.Add((Control) this.priorityLabel);
      this.Controls.Add((Control) this.colorNameTxtBx);
      this.Controls.Add((Control) this.switchPanel);
      this.Margin = new Padding(0);
      this.Name = nameof (PicPalette);
      this.Size = new Size(51, 294);
      this.Click += new EventHandler(this.PicPalette_Click);
      this.MouseDown += new MouseEventHandler(this.PicPalette_MouseDown);
      this.switchPanel.ResumeLayout(false);
      this.switchPanel.PerformLayout();
      ((ISupportInitialize) this.blackBox).EndInit();
      ((ISupportInitialize) this.blueBox).EndInit();
      ((ISupportInitialize) this.greenBox).EndInit();
      ((ISupportInitialize) this.cyanBox).EndInit();
      ((ISupportInitialize) this.redBox).EndInit();
      ((ISupportInitialize) this.magentaBox).EndInit();
      ((ISupportInitialize) this.brownBox).EndInit();
      ((ISupportInitialize) this.brightGrayBox).EndInit();
      ((ISupportInitialize) this.grayBox).EndInit();
      ((ISupportInitialize) this.brightBlueBox).EndInit();
      ((ISupportInitialize) this.brightGreenBox).EndInit();
      ((ISupportInitialize) this.brightCyanBox).EndInit();
      ((ISupportInitialize) this.brightRedBox).EndInit();
      ((ISupportInitialize) this.brightMagentaBox).EndInit();
      ((ISupportInitialize) this.whiteBox).EndInit();
      ((ISupportInitialize) this.yellowBox).EndInit();
      ((ISupportInitialize) this.selectedColorBox).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
