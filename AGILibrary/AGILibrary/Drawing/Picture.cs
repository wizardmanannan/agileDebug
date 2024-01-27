// Decompiled with JetBrains decompiler
// Type: AGI.Drawing.Picture
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace AGI.Drawing
{
  public class Picture
  {
    protected Picture.DrawState currentDrawState;
    protected ArrayList savedDrawStates;
    public static Rectangle ClientRect = new Rectangle(0, 0, Resource.Picture.Size.Width - 1, Resource.Picture.Size.Height - 1);
    private static byte[][] circles = new byte[8][]
    {
      new byte[1]{ (byte) 128 },
      new byte[1]{ (byte) 252 },
      new byte[2]{ (byte) 95, (byte) 244 },
      new byte[4]
      {
        (byte) 102,
        byte.MaxValue,
        (byte) 246,
        (byte) 96
      },
      new byte[6]
      {
        (byte) 35,
        (byte) 191,
        byte.MaxValue,
        byte.MaxValue,
        (byte) 238,
        (byte) 32
      },
      new byte[9]
      {
        (byte) 49,
        (byte) 231,
        (byte) 158,
        byte.MaxValue,
        byte.MaxValue,
        (byte) 222,
        (byte) 121,
        (byte) 227,
        (byte) 0
      },
      new byte[12]
      {
        (byte) 56,
        (byte) 249,
        (byte) 243,
        (byte) 239,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        (byte) 254,
        (byte) 249,
        (byte) 243,
        (byte) 227,
        (byte) 128
      },
      new byte[15]
      {
        (byte) 24,
        (byte) 60,
        (byte) 126,
        (byte) 126,
        (byte) 126,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        (byte) 126,
        (byte) 126,
        (byte) 126,
        (byte) 60,
        (byte) 24
      }
    };
    private static byte[] splatterMap = new byte[32]
    {
      (byte) 32,
      (byte) 148,
      (byte) 2,
      (byte) 36,
      (byte) 144,
      (byte) 130,
      (byte) 164,
      (byte) 162,
      (byte) 130,
      (byte) 9,
      (byte) 10,
      (byte) 34,
      (byte) 18,
      (byte) 16,
      (byte) 66,
      (byte) 20,
      (byte) 145,
      (byte) 74,
      (byte) 145,
      (byte) 17,
      (byte) 8,
      (byte) 18,
      (byte) 37,
      (byte) 16,
      (byte) 34,
      (byte) 168,
      (byte) 20,
      (byte) 36,
      (byte) 0,
      (byte) 80,
      (byte) 36,
      (byte) 4
    };
    private static byte[] splatterStart = new byte[120]
    {
      (byte) 0,
      (byte) 24,
      (byte) 48,
      (byte) 196,
      (byte) 220,
      (byte) 101,
      (byte) 235,
      (byte) 72,
      (byte) 96,
      (byte) 189,
      (byte) 137,
      (byte) 5,
      (byte) 10,
      (byte) 244,
      (byte) 125,
      (byte) 125,
      (byte) 133,
      (byte) 176,
      (byte) 142,
      (byte) 149,
      (byte) 31,
      (byte) 34,
      (byte) 13,
      (byte) 223,
      (byte) 42,
      (byte) 120,
      (byte) 213,
      (byte) 115,
      (byte) 28,
      (byte) 180,
      (byte) 64,
      (byte) 161,
      (byte) 185,
      (byte) 60,
      (byte) 202,
      (byte) 88,
      (byte) 146,
      (byte) 52,
      (byte) 204,
      (byte) 206,
      (byte) 215,
      (byte) 66,
      (byte) 144,
      (byte) 15,
      (byte) 139,
      (byte) 127,
      (byte) 50,
      (byte) 237,
      (byte) 92,
      (byte) 157,
      (byte) 200,
      (byte) 153,
      (byte) 173,
      (byte) 78,
      (byte) 86,
      (byte) 166,
      (byte) 247,
      (byte) 104,
      (byte) 183,
      (byte) 37,
      (byte) 130,
      (byte) 55,
      (byte) 58,
      (byte) 81,
      (byte) 105,
      (byte) 38,
      (byte) 56,
      (byte) 82,
      (byte) 158,
      (byte) 154,
      (byte) 79,
      (byte) 167,
      (byte) 67,
      (byte) 16,
      (byte) 128,
      (byte) 238,
      (byte) 61,
      (byte) 89,
      (byte) 53,
      (byte) 207,
      (byte) 121,
      (byte) 116,
      (byte) 181,
      (byte) 162,
      (byte) 177,
      (byte) 150,
      (byte) 35,
      (byte) 224,
      (byte) 190,
      (byte) 5,
      (byte) 245,
      (byte) 110,
      (byte) 25,
      (byte) 197,
      (byte) 102,
      (byte) 73,
      (byte) 240,
      (byte) 209,
      (byte) 84,
      (byte) 169,
      (byte) 112,
      (byte) 75,
      (byte) 164,
      (byte) 226,
      (byte) 230,
      (byte) 229,
      (byte) 171,
      (byte) 228,
      (byte) 210,
      (byte) 170,
      (byte) 76,
      (byte) 227,
      (byte) 6,
      (byte) 111,
      (byte) 198,
      (byte) 74,
      (byte) 164,
      (byte) 117,
      (byte) 151,
      (byte) 225
    };

    public Bitmap VisualBitmap => this.currentDrawState.visualBitmap;

    public Bitmap PriorityBitmap => this.currentDrawState.priorityBitmap;

    protected BitmapData VisualBitmapData
    {
      get => this.currentDrawState.visualBitmapData;
      set => this.currentDrawState.visualBitmapData = value;
    }

    protected BitmapData PriorityBitmapData
    {
      get => this.currentDrawState.priorityBitmapData;
      set => this.currentDrawState.priorityBitmapData = value;
    }

    public AGI.Color VisualColor
    {
      set => this.currentDrawState.visualColor = value;
    }

    public AGI.Color PriorityColor
    {
      set => this.currentDrawState.priorityColor = value;
    }

    public byte PenSize
    {
      set => this.currentDrawState.penSize = value;
    }

    public bool PenIsRectangle
    {
      set => this.currentDrawState.penIsRectangle = value;
    }

    public bool PenIsSplatter
    {
      set => this.currentDrawState.penIsSplatter = value;
    }

    public bool VisualDraw
    {
      set => this.currentDrawState.visualDraw = value;
    }

    public bool PriorityDraw
    {
      set => this.currentDrawState.priorityDraw = value;
    }

    public Picture()
    {
      this.currentDrawState = new Picture.DrawState();
      this.savedDrawStates = new ArrayList();
      this.Clear();
    }

    public void Clear()
    {
      this.VisualDraw = true;
      this.PriorityDraw = true;
      this.VisualColor = AGI.Color.White;
      this.PriorityColor = AGI.Color.Red;
      this.BeginDraw(false);
      for (int y = 0; y < this.VisualBitmap.Height; ++y)
      {
        for (int x = 0; x < this.VisualBitmap.Width; ++x)
          this.Plot(x, y);
      }
      this.EndDraw();
      this.VisualDraw = false;
      this.PriorityDraw = false;
      this.PenSize = (byte) 0;
      this.PenIsRectangle = false;
      this.PenIsSplatter = false;
    }

    public void BeginDraw(bool saveState)
    {
      if (saveState)
        this.savedDrawStates.Add((object) new Picture.DrawState(this.currentDrawState));
      else
        this.savedDrawStates.Clear();
      this.VisualBitmapData = this.VisualBitmap.LockBits(new Rectangle(0, 0, this.VisualBitmap.Width, this.VisualBitmap.Height), ImageLockMode.ReadWrite, this.VisualBitmap.PixelFormat);
      this.PriorityBitmapData = this.PriorityBitmap.LockBits(new Rectangle(0, 0, this.PriorityBitmap.Width, this.PriorityBitmap.Height), ImageLockMode.ReadWrite, this.PriorityBitmap.PixelFormat);
    }

    public void EndDraw()
    {
      this.VisualBitmap.UnlockBits(this.VisualBitmapData);
      this.VisualBitmapData = (BitmapData) null;
      this.PriorityBitmap.UnlockBits(this.PriorityBitmapData);
      this.PriorityBitmapData = (BitmapData) null;
    }

    public void RevertDraw()
    {
      if (this.savedDrawStates.Count <= 0)
        return;
      this.currentDrawState = (Picture.DrawState) this.savedDrawStates[this.savedDrawStates.Count - 1];
      this.savedDrawStates.Remove((object) this.currentDrawState);
    }

    public void DrawCommands(ArrayList commandStack)
    {
      this.BeginDraw(false);
      foreach (Resource.Picture.Command command in commandStack)
        command.Draw(this);
      this.EndDraw();
    }

    public unsafe void Plot(int x, int y)
    {
      if (y * 160 + x >= 26880)
        return;
      if (this.currentDrawState.visualDraw)
        *(sbyte*) ((IntPtr) (void*) this.VisualBitmapData.Scan0 + y * this.VisualBitmapData.Stride + x) = (sbyte) this.currentDrawState.visualColor.ColorIndex;
      if (!this.currentDrawState.priorityDraw)
        return;
      *(sbyte*) ((IntPtr) (void*) this.PriorityBitmapData.Scan0 + y * this.PriorityBitmapData.Stride + x) = (sbyte) this.currentDrawState.priorityColor.ColorIndex;
    }

    public unsafe void Plot(Point point)
    {
      if (point.Y * 160 + point.X >= 26880)
        return;
      if (this.currentDrawState.visualDraw)
        *(sbyte*) ((IntPtr) (void*) this.VisualBitmapData.Scan0 + point.Y * this.VisualBitmapData.Stride + point.X) = (sbyte) this.currentDrawState.visualColor.ColorIndex;
      if (!this.currentDrawState.priorityDraw)
        return;
      *(sbyte*) ((IntPtr) (void*) this.PriorityBitmapData.Scan0 + point.Y * this.PriorityBitmapData.Stride + point.X) = (sbyte) this.currentDrawState.priorityColor.ColorIndex;
    }

    public void Line(Point start, Point end)
    {
      int x1 = start.X;
      int y1 = start.Y;
      int x2 = end.X;
      int y2 = end.Y;
      int num1 = y1;
      int num2 = x1;
      if (num1 == y2)
        this.XLine(num2, x2, num1);
      else if (num2 == x2)
      {
        this.YLine(num1, y2, num2);
      }
      else
      {
        int num3 = 1;
        int num4 = y2 - y1;
        if (num4 < 0)
        {
          num3 *= -1;
          num4 *= -1;
        }
        int num5 = 1;
        int num6 = x2 - x1;
        if (num6 < 0)
        {
          num5 *= -1;
          num6 *= -1;
        }
        int num7;
        int num8;
        int num9;
        int num10;
        if (num6 >= num4)
        {
          num7 = num6;
          num8 = num6;
          num9 = num6 / 2;
          num10 = 0;
        }
        else
        {
          num7 = num4;
          num8 = num4;
          num10 = num4 / 2;
          num9 = 0;
        }
        do
        {
          num9 += num4;
          if (num9 >= num8)
          {
            num9 -= num8;
            num1 += num3;
          }
          num10 += num6;
          if (num10 >= num8)
          {
            num10 -= num8;
            num2 += num5;
          }
          this.Plot(num2, num1);
          --num7;
        }
        while (num7 > 0);
      }
    }

    public void XLine(int startX, int endX, int y)
    {
      if (startX > endX)
      {
        int num = startX;
        startX = endX;
        endX = num;
      }
      for (int x = startX; x <= endX; ++x)
        this.Plot(x, y);
    }

    public void YLine(int startY, int endY, int x)
    {
      if (startY > endY)
      {
        int num = startY;
        startY = endY;
        endY = num;
      }
      for (int y = startY; y <= endY; ++y)
        this.Plot(x, y);
    }

    public unsafe void Fill(Point fillPoint)
    {
      if (!this.currentDrawState.visualDraw && !this.currentDrawState.priorityDraw || this.currentDrawState.visualDraw && (int) this.currentDrawState.visualColor.ColorIndex == (int) AGI.Color.White.ColorIndex || this.currentDrawState.priorityDraw && !this.currentDrawState.visualDraw && (int) this.currentDrawState.priorityColor.ColorIndex == (int) AGI.Color.Red.ColorIndex)
        return;
      ArrayList arrayList = new ArrayList();
      byte colorIndex;
      BitmapData bitmapData;
      if (!this.currentDrawState.visualDraw)
      {
        colorIndex = AGI.Color.Red.ColorIndex;
        bitmapData = this.PriorityBitmapData;
      }
      else
      {
        colorIndex = AGI.Color.White.ColorIndex;
        bitmapData = this.VisualBitmapData;
      }
      arrayList.Add((object) fillPoint);
      do
      {
        fillPoint = (Point) arrayList[arrayList.Count - 1];
        arrayList.RemoveAt(arrayList.Count - 1);
        int x = fillPoint.X;
        int y = fillPoint.Y;
        if ((int) *(byte*) ((IntPtr) (void*) bitmapData.Scan0 + y * bitmapData.Stride + x) == (int) colorIndex)
        {
          this.Plot(fillPoint);
          if (fillPoint.Y > 0 && (int) *(byte*) ((IntPtr) (void*) bitmapData.Scan0 + (y - 1) * bitmapData.Stride + x) == (int) colorIndex)
            arrayList.Add((object) new Point(x, y - 1));
          if (fillPoint.X > 0 && (int) *(byte*) ((IntPtr) (void*) bitmapData.Scan0 + y * bitmapData.Stride + (x - 1)) == (int) colorIndex)
            arrayList.Add((object) new Point(x - 1, y));
          if (fillPoint.X < 159 && (int) *(byte*) ((IntPtr) (void*) bitmapData.Scan0 + y * bitmapData.Stride + (x + 1)) == (int) colorIndex)
            arrayList.Add((object) new Point(x + 1, y));
          if (fillPoint.Y < 167 && (int) *(byte*) ((IntPtr) (void*) bitmapData.Scan0 + (y + 1) * bitmapData.Stride + x) == (int) colorIndex)
            arrayList.Add((object) new Point(x, y + 1));
        }
      }
      while (arrayList.Count > 0);
    }

    public void PlotPattern(int patternNum, Point point)
    {
      int num1 = 0;
      int num2 = (int) Picture.splatterStart[patternNum >> 1 & (int) sbyte.MaxValue];
      byte penSize = this.currentDrawState.penSize;
      int num3 = point.X;
      int num4 = point.Y;
      if (num3 < ((int) penSize + 1) / 2)
        num3 = ((int) penSize + 1) / 2;
      else if (num3 > 160 - ((int) penSize / 2 + 1))
        num3 = 160 - ((int) penSize / 2 + 1);
      if (num4 < (int) penSize)
        num4 = (int) penSize;
      else if (num4 >= 168 - (int) penSize)
        num4 = 167 - (int) penSize;
      for (int y = num4 - (int) penSize; y <= num4 + (int) penSize; ++y)
      {
        for (int x = num3 - (int) Math.Ceiling((double) penSize / 2.0); x <= num3 + (int) Math.Floor((double) penSize / 2.0); ++x)
        {
          if (this.currentDrawState.penIsRectangle)
          {
            if (this.currentDrawState.penIsSplatter)
            {
              if (((int) Picture.splatterMap[num2 >> 3] >> 7 - (num2 & 7) & 1) > 0)
                this.Plot(x, y);
              ++num2;
              if (num2 == (int) byte.MaxValue)
                num2 = 0;
            }
            else
              this.Plot(x, y);
          }
          else
          {
            if (((int) Picture.circles[(int) penSize][num1 >> 3] >> 7 - (num1 & 7) & 1) > 0)
            {
              if (this.currentDrawState.penIsSplatter)
              {
                if (((int) Picture.splatterMap[num2 >> 3] >> 7 - (num2 & 7) & 1) > 0)
                  this.Plot(x, y);
                ++num2;
                if (num2 == (int) byte.MaxValue)
                  num2 = 0;
              }
              else
                this.Plot(x, y);
            }
            ++num1;
          }
        }
      }
    }

    protected class DrawState
    {
      public Bitmap visualBitmap;
      public Bitmap priorityBitmap;
      public AGI.Color visualColor;
      public AGI.Color priorityColor;
      public byte penSize;
      public bool penIsRectangle;
      public bool penIsSplatter;
      public bool visualDraw;
      public bool priorityDraw;
      public BitmapData visualBitmapData;
      public BitmapData priorityBitmapData;

      public DrawState()
      {
        this.visualBitmap = new Bitmap(Resource.Picture.Size.Width, Resource.Picture.Size.Height, PixelFormat.Format8bppIndexed);
        this.priorityBitmap = new Bitmap(Resource.Picture.Size.Width, Resource.Picture.Size.Height, PixelFormat.Format8bppIndexed);
        this.visualBitmap.Palette = AGI.Color.GetSystemPalette();
        this.priorityBitmap.Palette = AGI.Color.GetSystemPalette();
        this.visualBitmapData = (BitmapData) null;
        this.priorityBitmapData = (BitmapData) null;
      }

      public DrawState(Picture.DrawState original)
      {
        this.visualBitmap = (Bitmap) original.visualBitmap.Clone();
        this.priorityBitmap = (Bitmap) original.priorityBitmap.Clone();
        this.visualColor = original.visualColor;
        this.priorityColor = original.priorityColor;
        this.penSize = original.penSize;
        this.penIsRectangle = original.penIsRectangle;
        this.penIsSplatter = original.penIsSplatter;
        this.visualDraw = original.visualDraw;
        this.priorityDraw = original.priorityDraw;
      }

      protected ColorPalette GetColorPalette(uint nColors)
      {
        PixelFormat format = PixelFormat.Format1bppIndexed;
        if (nColors > 2U)
          format = PixelFormat.Format4bppIndexed;
        if (nColors > 16U)
          format = PixelFormat.Format8bppIndexed;
        Bitmap bitmap = new Bitmap(1, 1, format);
        ColorPalette palette = bitmap.Palette;
        bitmap.Dispose();
        return palette;
      }
    }
  }
}
