// Decompiled with JetBrains decompiler
// Type: AGI.Drawing.View
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AGI.Drawing
{
  public class View
  {
    protected Bitmap bitmap;
    protected BitmapData bitmapData;

    public Bitmap Bitmap => this.bitmap;

    public View(Size bitmapSize)
    {
      this.bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height, PixelFormat.Format8bppIndexed);
      this.bitmap.Palette = AGI.Color.GetSystemPalette();
    }

    public void BeginDraw() => this.bitmapData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadWrite, this.bitmap.PixelFormat);

    public void EndDraw()
    {
      this.bitmap.UnlockBits(this.bitmapData);
      this.bitmapData = (BitmapData) null;
    }

    public unsafe void Plot(int x, int y, AGI.Color color) => *(sbyte*) ((IntPtr) (void*) this.bitmapData.Scan0 + y * this.bitmapData.Stride + x) = (sbyte) (byte) color;

    public unsafe void Plot(Point point, AGI.Color color) => *(sbyte*) ((IntPtr) (void*) this.bitmapData.Scan0 + point.Y * this.bitmapData.Stride + point.X) = (sbyte) (byte) color;

    public unsafe AGI.Color GetPixel(int x, int y) => *(byte*) ((IntPtr) (void*) this.bitmapData.Scan0 + y * this.bitmapData.Stride + x);

    public unsafe AGI.Color GetPixel(Point point) => *(byte*) ((IntPtr) (void*) this.bitmapData.Scan0 + point.Y * this.bitmapData.Stride + point.X);
  }
}
