// Decompiled with JetBrains decompiler
// Type: AGI.ColorPicker
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AGI
{
  public class ColorPicker : UserControl
  {
    private Color[] selectedColor = new Color[2]
    {
      Color.White,
      Color.Black
    };
    private ArrayList colorRectangles = new ArrayList();
    private int colorPadding = 4;
    private System.ComponentModel.Container components;

    public ColorPicker() => this.InitializeComponent();

    protected override void OnPaint(PaintEventArgs e)
    {
      int num = 16;
      Rectangle rect1 = new Rectangle(1, 2, num * 2 + this.colorPadding, num * 2 + this.colorPadding);
      e.Graphics.FillRectangle(SystemBrushes.ControlDark, rect1);
      e.Graphics.DrawRectangle(SystemPens.ControlLight, rect1);
      Rectangle rect2 = new Rectangle(rect1.Left + (rect1.Width / 2 - num / 2 - num / 3), rect1.Top + num / 4, num, num);
      Rectangle rect3 = new Rectangle(rect2.Left + num / 2, rect2.Bottom - num / 2, num, num);
      e.Graphics.FillRectangle((Brush) new SolidBrush((System.Drawing.Color) this.selectedColor[1]), rect3);
      e.Graphics.DrawRectangle(SystemPens.ControlLight, rect3);
      e.Graphics.FillRectangle((Brush) new SolidBrush((System.Drawing.Color) this.selectedColor[0]), rect2);
      e.Graphics.DrawRectangle(SystemPens.ControlLight, rect2);
      this.colorRectangles.Clear();
      for (int index = 0; index < Color.Palette.Length; ++index)
      {
        Rectangle rect4 = new Rectangle(rect1.Left + this.colorPadding * (index % 2) + index % 2 * num, rect1.Bottom + this.colorPadding + this.colorPadding * (index / 2) + index / 2 * num, num, num);
        e.Graphics.FillRectangle((Brush) new SolidBrush(Color.Palette[index]), rect4);
        e.Graphics.DrawRectangle(SystemPens.ControlDark, rect4);
        this.colorRectangles.Add((object) rect4);
      }
    }

    private void ColorPicker_MouseDown(object sender, MouseEventArgs e)
    {
      for (byte index = 0; (int) index < this.colorRectangles.Count; ++index)
      {
        if (((Rectangle) this.colorRectangles[(int) index]).Contains(e.X, e.Y))
        {
          if (e.Button == MouseButtons.Left)
            this.LeftColor = index;
          else if (e.Button == MouseButtons.Right)
            this.RightColor = index;
        }
      }
    }

    [Category("Appearance")]
    [Description("The color selected for left mouse button")]
    [DefaultValue(15)]
    public byte LeftColor
    {
      get => (byte) this.selectedColor[0];
      set
      {
        this.selectedColor[0] = (Color) value;
        this.Invalidate();
      }
    }

    [Category("Appearance")]
    [Description("The color selected for right mouse button")]
    [DefaultValue(0)]
    public byte RightColor
    {
      get => (byte) this.selectedColor[1];
      set
      {
        this.selectedColor[1] = (Color) value;
        this.Invalidate();
      }
    }

    [Category("Layout")]
    [Description("Padding between color rectangles")]
    [DefaultValue(4)]
    public int ColorPadding
    {
      get => this.colorPadding;
      set
      {
        this.colorPadding = value;
        this.Invalidate();
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
      this.Name = nameof (ColorPicker);
      this.Size = new Size(64, 216);
      this.MouseDown += new MouseEventHandler(this.ColorPicker_MouseDown);
    }
  }
}
