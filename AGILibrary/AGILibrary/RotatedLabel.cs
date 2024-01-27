// Decompiled with JetBrains decompiler
// Type: AGI.RotatedLabel
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AGI
{
  internal class RotatedLabel : Label
  {
    private int m_RotateAngle;
    private string m_NewText = string.Empty;

    public int RotateAngle { get; set; }

    public string NewText { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
      Brush brush = (Brush) new SolidBrush(this.ForeColor);
      SizeF sizeF = e.Graphics.MeasureString(this.NewText, this.Font, this.Parent.Width);
      int num1 = (this.RotateAngle % 360 + 360) % 360;
      double num2 = ((Func<double, double>) (angle => Math.PI * angle / 180.0))((double) num1);
      int num3 = (int) Math.Ceiling((double) sizeF.Height * Math.Sin(num2));
      int num4 = (int) Math.Ceiling((double) sizeF.Width * Math.Cos(num2));
      int num5 = (int) Math.Ceiling((double) sizeF.Width * Math.Sin(num2));
      int num6 = (int) Math.Ceiling((double) sizeF.Height * Math.Cos(num2));
      int num7 = Math.Abs(num3) + Math.Abs(num4);
      int num8 = Math.Abs(num5) + Math.Abs(num6);
      this.Width = num7;
      this.Height = num8;
      int num9 = num1 < 0 || num1 >= 90 ? (num1 < 90 || num1 >= 180 ? (num1 < 180 || num1 >= 270 ? (num1 < 270 || num1 >= 360 ? 0 : 4) : 3) : 2) : 1;
      int dx = 0;
      int dy = 0;
      switch (num9)
      {
        case 1:
          dx = Math.Abs(num3);
          break;
        case 2:
          dx = num7;
          dy = Math.Abs(num6);
          break;
        case 3:
          dx = Math.Abs(num4);
          dy = num8;
          break;
        case 4:
          dy = Math.Abs(num5);
          break;
      }
      e.Graphics.TranslateTransform((float) dx, (float) dy);
      e.Graphics.RotateTransform((float) this.RotateAngle);
      e.Graphics.DrawString(this.NewText, this.Font, brush, 0.0f, 0.0f);
      base.OnPaint(e);
    }
  }
}
