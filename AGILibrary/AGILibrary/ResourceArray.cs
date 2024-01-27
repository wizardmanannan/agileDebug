// Decompiled with JetBrains decompiler
// Type: AGI.ResourceArray
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Collections;

namespace AGI
{
  public class ResourceArray : CollectionBase
  {
    public Resource this[int index]
    {
      get
      {
        foreach (Resource resource in (IEnumerable) this.List)
        {
          if (resource.Index == index)
            return resource;
        }
        return (Resource) null;
      }
      set
      {
        value.Index = !this.Contains(index) ? index : throw new ArgumentException("There is already a resource with this index!", nameof (index));
        this.List.Add((object) value);
      }
    }

    public int Add(Resource value)
    {
      value.Index = this.LargestIndex + 1;
      this.List.Add((object) value);
      return value.Index;
    }

    public int IndexOf(Resource value)
    {
      if (!this.List.Contains((object) value))
        return -1;
      foreach (Resource resource in (IEnumerable) this.List)
      {
        if (resource == value)
          return resource.Index;
      }
      return -1;
    }

    public void Insert(int index, Resource value) => this.List.Insert(index, (object) value);

    public void Remove(Resource value) => this.List.Remove((object) value);

    public bool Contains(Resource value) => this.List.Contains((object) value);

    public bool Contains(int index)
    {
      foreach (Resource resource in (IEnumerable) this.List)
      {
        if (resource.Index == index)
          return true;
      }
      return false;
    }

    public int LargestIndex
    {
      get
      {
        int val2 = -1;
        foreach (Resource resource in (CollectionBase) this)
          val2 = Math.Max(resource.Index, val2);
        return val2;
      }
    }

    protected override void OnInsert(int index, object value)
    {
      if (!value.GetType().ToString().StartsWith("AGI.Resource"))
        throw new ArgumentException("value must be of type AGI.Resource.", nameof (value));
      if (this.Contains((Resource) value))
        throw new ArgumentException("This resource already exists with a different index!", "volume");
    }

    protected override void OnRemove(int index, object value)
    {
      if (!value.GetType().ToString().StartsWith("AGI.Resource"))
        throw new ArgumentException("value must be of type AGI.Resource.", nameof (value));
    }

    protected override void OnSet(int index, object oldValue, object newValue)
    {
      if (!newValue.GetType().ToString().StartsWith("AGI.Resource"))
        throw new ArgumentException("newValue must be of type AGI.Resource.", nameof (newValue));
      if (this.Contains((Resource) newValue))
        throw new ArgumentException("This resource already exists with a different index!", "volume");
    }

    protected override void OnValidate(object value)
    {
      if (!value.GetType().ToString().StartsWith("AGI.Resource"))
        throw new ArgumentException("value must be of type AGI.Resource.");
    }
  }
}
