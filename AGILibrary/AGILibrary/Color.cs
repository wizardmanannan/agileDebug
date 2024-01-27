// Decompiled with JetBrains decompiler
// Type: AGI.Color
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace AGI
{
  [TypeConverter(typeof (ColorConverter))]
  [Serializable]
  public class Color
  {
    protected byte colorIndex;
    public static System.Drawing.Color[] Palette = new System.Drawing.Color[16]
    {
      System.Drawing.Color.FromArgb(0, 0, 0),
      System.Drawing.Color.FromArgb(0, 0, 170),
      System.Drawing.Color.FromArgb(0, 170, 0),
      System.Drawing.Color.FromArgb(0, 170, 170),
      System.Drawing.Color.FromArgb(170, 0, 0),
      System.Drawing.Color.FromArgb(170, 0, 170),
      System.Drawing.Color.FromArgb(170, 85, 0),
      System.Drawing.Color.FromArgb(170, 170, 170),
      System.Drawing.Color.FromArgb(85, 85, 85),
      System.Drawing.Color.FromArgb(85, 85, (int) byte.MaxValue),
      System.Drawing.Color.FromArgb(85, (int) byte.MaxValue, 85),
      System.Drawing.Color.FromArgb(85, (int) byte.MaxValue, (int) byte.MaxValue),
      System.Drawing.Color.FromArgb((int) byte.MaxValue, 85, 85),
      System.Drawing.Color.FromArgb((int) byte.MaxValue, 85, (int) byte.MaxValue),
      System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 85),
      System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
    };

    public byte ColorIndex
    {
      get => this.colorIndex;
      set => this.colorIndex = TypeDescriptor.GetConverter((object) this).IsValid((object) value) ? value : throw new ArgumentException("Color index must be between 0 and " + Color.Palette.Length.ToString(), nameof (ColorIndex));
    }

    public static ColorPalette GetSystemPalette()
    {
      Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
      ColorPalette palette = bitmap.Palette;
      bitmap.Dispose();
      for (int index = 0; index < Color.Palette.Length; ++index)
        palette.Entries[index] = Color.Palette[index];
      return palette;
    }

    public Color(byte index) => this.ColorIndex = index;

    public override bool Equals(object obj) => (object) (obj as Color) != null && (int) ((Color) obj).ColorIndex == (int) this.ColorIndex;

    public override int GetHashCode() => (int) this.ColorIndex;

    public static explicit operator System.Drawing.Color(Color color) => (System.Drawing.Color) TypeDescriptor.GetConverter((object) color).ConvertTo((object) color, typeof (System.Drawing.Color));

    public static implicit operator Color(byte index) => (Color) TypeDescriptor.GetConverter(typeof (Color)).ConvertFrom((object) index);

    public static explicit operator byte(Color color) => (byte) TypeDescriptor.GetConverter((object) color).ConvertTo((object) color, typeof (byte));

    public static bool operator ==(Color col1, Color col2) => (int) col1.ColorIndex == (int) col2.ColorIndex;

    public static bool operator !=(Color col1, Color col2) => (int) col1.ColorIndex != (int) col2.ColorIndex;

    public static Color Black => new Color((byte) 0);

    public static Color Blue => new Color((byte) 1);

    public static Color Green => new Color((byte) 2);

    public static Color Cyan => new Color((byte) 3);

    public static Color Red => new Color((byte) 4);

    public static Color Magenta => new Color((byte) 5);

    public static Color Brown => new Color((byte) 6);

    public static Color BrightGray => new Color((byte) 7);

    public static Color Gray => new Color((byte) 8);

    public static Color BrightBlue => new Color((byte) 9);

    public static Color BrightGreen => new Color((byte) 10);

    public static Color BrightCyan => new Color((byte) 11);

    public static Color BrightRed => new Color((byte) 12);

    public static Color BrightMagenta => new Color((byte) 13);

    public static Color Yellow => new Color((byte) 14);

    public static Color White => new Color((byte) 15);
  }
}
