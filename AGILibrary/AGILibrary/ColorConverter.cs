// Decompiled with JetBrains decompiler
// Type: AGI.ColorConverter
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.ComponentModel;
using System.Globalization;

namespace AGI
{
  public class ColorConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof (byte) || base.CanConvertFrom(context, sourceType);

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof (byte) || destinationType == typeof (System.Drawing.Color) || base.CanConvertTo(context, destinationType);

    public override object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      return value is byte index ? (object) new Color(index) : base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      if (destinationType == typeof (byte))
        return (object) ((Color) value).ColorIndex;
      return destinationType == typeof (System.Drawing.Color) ? (object) Color.Palette[(int) ((Color) value).ColorIndex] : base.ConvertTo(context, culture, value, destinationType);
    }

    public override bool IsValid(ITypeDescriptorContext context, object value)
    {
      if (!(value is byte num))
        return base.IsValid(context, value);
      return num > (byte) 0 || (int) (byte) value < Color.Palette.Length;
    }
  }
}
