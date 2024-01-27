// Decompiled with JetBrains decompiler
// Type: AGI.Volume
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;

namespace AGI
{
  public class Volume : IDisposable
  {
    private bool disposed;
    public int Index;
    protected ResourceArray pictureArray = new ResourceArray();
    protected ResourceArray viewArray = new ResourceArray();
    protected ResourceArray soundArray = new ResourceArray();
    protected ResourceArray logicArray = new ResourceArray();

    public ResourceArray Pictures => this.pictureArray;

    public ResourceArray Views => this.viewArray;

    public ResourceArray Sounds => this.soundArray;

    public ResourceArray Logics => this.logicArray;

    public ResourceArray this[Game.Directory dirType]
    {
      get
      {
        switch (dirType)
        {
          case Game.Directory.LogDir:
            return this.Logics;
          case Game.Directory.PicDir:
            return this.Pictures;
          case Game.Directory.ViewDir:
            return this.Views;
          case Game.Directory.SndDir:
            return this.Sounds;
          default:
            throw new ArgumentException("No handler for this dirType implemented!", nameof (dirType));
        }
      }
    }

    public void Dispose()
    {
      if (this.disposed)
        return;
      this.pictureArray.Clear();
      this.pictureArray = (ResourceArray) null;
      this.viewArray.Clear();
      this.viewArray = (ResourceArray) null;
      this.soundArray.Clear();
      this.soundArray = (ResourceArray) null;
      this.logicArray.Clear();
      this.logicArray = (ResourceArray) null;
      this.disposed = true;
    }
  }
}
