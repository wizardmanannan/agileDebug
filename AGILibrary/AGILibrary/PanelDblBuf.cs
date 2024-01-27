// Decompiled with JetBrains decompiler
// Type: AGI.PanelDblBuf
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System.Windows.Forms;

namespace AGI
{
  public class PanelDblBuf : Panel
  {
    private System.ComponentModel.Container components;

    private void InitializeComponent() => this.components = new System.ComponentModel.Container();

    public PanelDblBuf()
    {
      this.InitializeComponent();
      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }
  }
}
