// Decompiled with JetBrains decompiler
// Type: AGI.LzwExpander
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System.IO;

namespace AGI
{
  public class LzwExpander
  {
    private const int MAX_BITS = 12;
    private const int TABLE_SIZE = 18041;
    private const int START_BITS = 9;
    private bool endOfStream;
    private Stream input;
    private int bits;
    private int maxValues;
    private int maxCodes;
    private byte[] appendChars = new byte[18041];
    private byte[] decodeStack = new byte[8192];
    private int decodeStackSize = -1;
    private int[] prefixCode = new int[18041];
    private int bitCount;
    private long bitBuffer;
    private int unext;
    private int unew;
    private int uold;
    private int ubits;
    private int uc;

    protected int SetBits(int value)
    {
      if (value == 12)
        return 1;
      this.bits = value;
      this.maxValues = (1 << this.bits) - 1;
      this.maxCodes = this.maxValues - 1;
      return 0;
    }

    protected int InputCode()
    {
      long bitBuffer = this.bitBuffer;
      int bitCount;
      for (bitCount = this.bitCount; bitCount <= 24; bitCount += 8)
      {
        long num1 = (long) this.input.ReadByte();
        if (num1 < 0L)
        {
          if (bitCount == 0)
            throw new IOException("End of stream reached!");
          break;
        }
        long num2 = num1 << bitCount;
        bitBuffer |= num2;
      }
      int num = (int) (bitBuffer & (long) short.MaxValue) % (1 << this.bits);
      this.bitBuffer = bitBuffer >> this.bits;
      this.bitCount = bitCount - this.bits;
      return num;
    }

    private int DecodeString(int offset, int code)
    {
      int num = 0;
      while (code > (int) byte.MaxValue)
      {
        this.decodeStack[offset] = this.appendChars[code];
        ++offset;
        code = this.prefixCode[code];
        if (num++ >= 4000)
          throw new IOException("Fatal error during code expansion");
      }
      this.decodeStack[offset] = (byte) code;
      return offset;
    }

    private void Unpack()
    {
      if (this.endOfStream || this.decodeStackSize > 0)
        return;
      if (this.unew == 257)
        this.endOfStream = true;
      else if (this.unew == 256)
      {
        this.unext = 258;
        this.ubits = this.SetBits(9);
        this.uold = this.InputCode();
        this.uc = this.uold;
        this.decodeStack[0] = (byte) this.uc;
        this.decodeStackSize = 0;
        this.unew = this.InputCode();
      }
      else
      {
        if (this.unew >= this.unext)
        {
          this.decodeStack[0] = (byte) this.uc;
          this.decodeStackSize = this.DecodeString(1, this.uold);
        }
        else
          this.decodeStackSize = this.DecodeString(0, this.unew);
        this.uc = (int) this.decodeStack[this.decodeStackSize];
        if (this.unext > this.maxCodes)
          this.ubits = this.SetBits(this.bits + 1);
        this.prefixCode[this.unext] = this.uold;
        this.appendChars[this.unext] = (byte) this.uc;
        ++this.unext;
        this.uold = this.unew;
        this.unew = this.InputCode();
      }
    }

    public bool Expand(byte[] compressedBuffer, byte[] uncompressedBuffer)
    {
      int index = 0;
      int length = uncompressedBuffer.Length;
      this.bitCount = 0;
      this.bitBuffer = 0L;
      this.endOfStream = false;
      this.decodeStackSize = -1;
      this.input = (Stream) new MemoryStream(compressedBuffer);
      this.ubits = this.SetBits(9);
      this.unext = 257;
      this.uold = this.InputCode();
      this.uc = this.uold;
      this.unew = this.InputCode();
      while (!this.endOfStream)
      {
        if (this.decodeStackSize >= 0)
        {
          for (; this.decodeStackSize >= 0 && length > 0; --length)
          {
            uncompressedBuffer[index] = this.decodeStack[this.decodeStackSize];
            --this.decodeStackSize;
            ++index;
          }
          if (length == 0)
            break;
        }
        try
        {
          this.Unpack();
        }
        catch (IOException ex)
        {
          this.endOfStream = true;
        }
      }
      return true;
    }
  }
}
