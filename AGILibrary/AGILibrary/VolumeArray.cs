// Decompiled with JetBrains decompiler
// Type: AGI.VolumeArray
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Collections;

namespace AGI
{
  public class VolumeArray : CollectionBase
  {
    public Volume this[int index]
    {
      get
      {
        foreach (Volume volume in (IEnumerable) this.List)
        {
          if (volume.Index == index)
            return volume;
        }
        return (Volume) null;
      }
      set
      {
        value.Index = !this.Contains(index) ? index : throw new ArgumentException("There is already a volume with this index!", nameof (index));
        this.List.Add((object) value);
      }
    }

    public int Add(Volume value)
    {
      value.Index = this.LargestIndex + 1;
      this.List.Add((object) value);
      return value.Index;
    }

    public int IndexOf(Volume value)
    {
      if (!this.List.Contains((object) value))
        return -1;
      foreach (Volume volume in (IEnumerable) this.List)
      {
        if (volume == value)
          return volume.Index;
      }
      return -1;
    }

    public void Insert(int index, Volume value) => this.List.Insert(index, (object) value);

    public void Remove(Volume value) => this.List.Remove((object) value);

    public bool Contains(Volume value) => this.List.Contains((object) value);

    public bool Contains(int index)
    {
      foreach (Volume volume in (IEnumerable) this.List)
      {
        if (volume.Index == index)
          return true;
      }
      return false;
    }

    public int LargestIndex
    {
      get
      {
        int val2 = -1;
        foreach (Volume volume in (CollectionBase) this)
          val2 = Math.Max(volume.Index, val2);
        return val2;
      }
    }

    protected override void OnInsert(int index, object value)
    {
      if (value.GetType() != Type.GetType("AGI.Volume"))
        throw new ArgumentException("value must be of type AGI.Volume.", nameof (value));
      if (this.Contains((Volume) value))
        throw new ArgumentException("This volume already exists with a different index!", "volume");
    }

    protected override void OnRemove(int index, object value)
    {
      if (value.GetType() != Type.GetType("AGI.Volume"))
        throw new ArgumentException("value must be of type AGI.Volume.", nameof (value));
    }

    protected override void OnSet(int index, object oldValue, object newValue)
    {
      if (newValue.GetType() != Type.GetType("AGI.Volume"))
        throw new ArgumentException("newValue must be of type AGI.Volume.", nameof (newValue));
      if (this.Contains((Volume) newValue))
        throw new ArgumentException("This volume already exists with a different index!", "volume");
    }

    protected override void OnValidate(object value)
    {
      if (value.GetType() != Type.GetType("AGI.Volume"))
        throw new ArgumentException("value must be of type AGI.Volume.", nameof (value));
    }
  }
}
