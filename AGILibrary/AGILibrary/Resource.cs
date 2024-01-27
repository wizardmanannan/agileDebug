// Decompiled with JetBrains decompiler
// Type: AGI.Resource
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace AGI
{
  [Serializable]
  public abstract class Resource
  {
    [NonSerialized]
    public bool IsModified;
    public bool IsLoaded;
    public int Index;

    public abstract byte[] Encode();

    protected void Crypt(byte[] rawData, int start, int end)
    {
      int num = 0;
      for (int index = start; index < end; ++index)
        rawData[index] ^= (byte) "Avis Durgan"[num++ % 11];
    }

    [Serializable]
    public class Objects : Resource
    {
      private List<Resource.Object> objects;

      public int Count => this.objects.Count;

      public byte NumOfAnimatedObjects { get; set; }

      public bool Crypted { get; set; }

      public Resource.Object this[int i]
      {
        get => this.objects[i];
        set => this.objects[i] = value;
      }

      public Objects(Resource.Objects objects, bool crypted = false)
      {
        this.NumOfAnimatedObjects = objects.NumOfAnimatedObjects;
        this.objects = new List<Resource.Object>();
        this.Crypted = crypted;
        foreach (Resource.Object @object in objects.objects)
          this.objects.Add(new Resource.Object(@object.Name, @object.Room));
      }

      public Objects(byte[] rawData)
      {
        this.objects = new List<Resource.Object>();
        this.Decode(rawData);
      }

      protected bool IsCrypt(byte[] rawData) => ((int) rawData[1] & 240) == 112;

      protected void Decode(byte[] rawData)
      {
        if (this.Crypted = this.IsCrypt(rawData))
          this.Crypt(rawData, 0, rawData.Length);
        int num1 = ((int) rawData[0] + ((int) rawData[1] << 8)) / 3;
        this.NumOfAnimatedObjects = rawData[2];
        this.objects.Capacity = num1;
        try
        {
          int num2 = 0;
          int index1 = 3;
          while (num2 < num1)
          {
            int index2 = (int) rawData[index1] + ((int) rawData[index1 + 1] << 8) + 3;
            int num3 = index2;
            do
              ;
            while (rawData[num3++] != (byte) 0);
            this.objects.Add(new Resource.Object(Encoding.ASCII.GetString(rawData, index2, num3 - index2 - 1), rawData[index1 + 2]));
            ++num2;
            index1 += 3;
          }
        }
        catch (IndexOutOfRangeException ex)
        {
        }
      }

      public override byte[] Encode()
      {
        MemoryStream memoryStream = new MemoryStream();
        int count = this.objects.Count;
        int num1 = count * 3;
        memoryStream.WriteByte((byte) (num1 & (int) byte.MaxValue));
        memoryStream.WriteByte((byte) (num1 >> 8 & (int) byte.MaxValue));
        memoryStream.WriteByte(this.NumOfAnimatedObjects);
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        List<string> stringList = new List<string>();
        int num2 = num1;
        for (int i = 0; i < count; ++i)
        {
          Resource.Object @object = this[i];
          int num3 = num2;
          if (dictionary.ContainsKey(@object.Name))
          {
            num3 = dictionary[@object.Name];
          }
          else
          {
            dictionary.Add(@object.Name, num3);
            stringList.Add(@object.Name);
            num2 += @object.Name.Length + 1;
          }
          memoryStream.WriteByte((byte) (num3 & (int) byte.MaxValue));
          memoryStream.WriteByte((byte) (num3 >> 8 & (int) byte.MaxValue));
          memoryStream.WriteByte(@object.Room);
        }
        foreach (string s in stringList)
        {
          foreach (byte num4 in Encoding.ASCII.GetBytes(s))
            memoryStream.WriteByte(num4);
          memoryStream.WriteByte((byte) 0);
        }
        byte[] array = memoryStream.ToArray();
        if (this.Crypted)
          this.Crypt(array, 0, array.Length);
        return array;
      }
    }

    [Serializable]
    public class Object
    {
      public string Name;
      public byte Room;

      public Object(string name, byte room)
      {
        this.Name = name;
        this.Room = room;
      }
    }

    [Serializable]
    public class Words : Resource
    {
      public Dictionary<string, int> WordToNumber { get; set; }

      public Dictionary<int, SortedSet<string>> NumberToWords { get; set; }

      public Words(byte[] rawData)
      {
        this.WordToNumber = new Dictionary<string, int>();
        this.NumberToWords = new Dictionary<int, SortedSet<string>>();
        this.Decode(rawData);
      }

      protected void Decode(byte[] rawData)
      {
        int num1 = 52;
        byte[] bytes = new byte[(int) byte.MaxValue];
        while (num1 < rawData.Length - 1)
        {
          byte[] numArray1 = rawData;
          int index1 = num1;
          int num2 = index1 + 1;
          int count = (int) numArray1[index1];
          int num3;
          do
          {
            byte[] numArray2 = bytes;
            int index2 = count++;
            byte[] numArray3 = rawData;
            int index3 = num2++;
            int num4 = (int) (byte) (((num3 = (int) numArray3[index3]) ^ (int) sbyte.MaxValue) & (int) sbyte.MaxValue);
            numArray2[index2] = (byte) num4;
          }
          while (num3 < 128);
          string wordText = Encoding.ASCII.GetString(bytes, 0, count);
          byte[] numArray4 = rawData;
          int index4 = num2;
          int num5 = index4 + 1;
          int num6 = (int) numArray4[index4] << 8;
          byte[] numArray5 = rawData;
          int index5 = num5;
          num1 = index5 + 1;
          int num7 = (int) numArray5[index5];
          this.AddWord(num6 + num7, wordText);
        }
      }

      public void AddWord(int wordNum, string wordText)
      {
        this.WordToNumber.Add(wordText, wordNum);
        SortedSet<string> sortedSet;
        if (this.NumberToWords.ContainsKey(wordNum))
        {
          sortedSet = this.NumberToWords[wordNum];
        }
        else
        {
          sortedSet = new SortedSet<string>();
          this.NumberToWords.Add(wordNum, sortedSet);
        }
        sortedSet.Add(wordText);
      }

      public override byte[] Encode() => throw new NotImplementedException();
    }

    [Serializable]
    public class Picture : Resource
    {
      public ArrayList CommandStack;
      public static Size Size = new Size(160, 168);
      [NonSerialized]
      public AGI.Drawing.Picture Screen;

      public Picture(byte[] rawData)
      {
        this.CommandStack = new ArrayList();
        this.Screen = new AGI.Drawing.Picture();
        this.Decode(rawData);
      }

      public Picture()
      {
        this.CommandStack = new ArrayList();
        this.Screen = new AGI.Drawing.Picture();
      }

      protected static void ValidateXYData(int x, int y)
      {
        if (x >= 240)
          throw new Exception("Resource data corrupt, expected X coordinate - found command.");
        if (y >= 240)
          throw new Exception("Resource data corrupt, expected Y coordinate - found command.");
        if (x >= Resource.Picture.Size.Width)
          throw new Exception("Resource data corrupt, X coordinate out of range.");
        if (y >= Resource.Picture.Size.Height)
          throw new Exception("Resource data corrupt, Y coordinate out of range. " + y.ToString());
      }

      public int Optimize()
      {
        int length = this.Encode().Length;
        for (int index = 0; index < this.CommandStack.Count; ++index)
        {
          Resource.Picture.Command command1 = (Resource.Picture.Command) this.CommandStack[index];
          Resource.Picture.Command[] commandArray = command1.Optimize();
          if (commandArray != null)
          {
            this.RemoveCommand(command1);
            foreach (Resource.Picture.Command command2 in commandArray)
              this.AddCommand(command2, index++);
          }
        }
        return this.Encode().Length - length;
      }

      public void AddCommand(Resource.Picture.Command command, int index)
      {
        this.CommandStack.Insert(index, (object) command);
        this.IsModified = true;
      }

      public void AddCommand(Resource.Picture.Command command)
      {
        this.CommandStack.Add((object) command);
        this.IsModified = true;
      }

      public void RemoveCommand(int index)
      {
        this.CommandStack.RemoveAt(index);
        this.IsModified = true;
      }

      public void RemoveCommand(Resource.Picture.Command command)
      {
        this.CommandStack.Remove((object) command);
        this.IsModified = true;
      }

      protected void Decode(byte[] rawData)
      {
        if (rawData == null)
          throw new ArgumentNullException(nameof (rawData), "The data to decode cannot be null.");
        if (rawData.Length < 2)
          throw new ArgumentException("There is not enough data to decode!", nameof (rawData));
        this.CommandStack.Clear();
        int num1 = 0;
        Resource.Picture.Command.PenType activePenType = (Resource.Picture.Command.PenType) null;
        while (num1 < rawData.Length)
        {
          byte[] numArray1 = rawData;
          int index1 = num1;
          int num2 = index1 + 1;
          byte num3 = numArray1[index1];
          if (((int) num3 & 240) != 240)
            throw new Exception(string.Format("Position {0}: Expected a command, found 0x{1:X}.", (object) (num2 - 1), (object) num3));
          if (num3 == byte.MaxValue)
          {
            if (num2 < 2)
              throw new Exception(string.Format("Position {0}: Expected a command, found end of picture data!", (object) (num2 - 1)));
            break;
          }
          byte[] numArray2 = rawData;
          int index2 = num2;
          int num4 = index2 + 1;
          byte num5 = numArray2[index2];
          int length = 1;
          for (; ((int) num5 & 240) != 240 && num4 < rawData.Length; num5 = rawData[num4++])
            ++length;
          num1 = num4 - 1;
          byte[] numArray3 = new byte[length];
          Array.Copy((Array) rawData, num1 - length, (Array) numArray3, 0, length);
          try
          {
            Resource.Picture.Command command = Resource.Picture.Command.GetCommand(numArray3, ref activePenType);
            if (command != null)
              this.CommandStack.Add((object) command);
          }
          catch (Exception ex)
          {
            throw new Exception(string.Format("Decode error in command 0x{1:X} at position {0}.", (object) (num1 - length), (object) num3), ex);
          }
        }
        this.IsModified = false;
      }

      public override byte[] Encode()
      {
        MemoryStream memoryStream = new MemoryStream();
        foreach (Resource.Picture.Command command in this.CommandStack)
        {
          byte[] rawData = command.GetRawData();
          if (rawData != null)
            memoryStream.Write(rawData, 0, rawData.Length);
        }
        memoryStream.WriteByte(byte.MaxValue);
        memoryStream.Capacity = (int) memoryStream.Length;
        return memoryStream.GetBuffer();
      }

      [Serializable]
      public abstract class Command
      {
        public ArrayList PointList;

        public Command() => this.PointList = new ArrayList();

        public static Resource.Picture.Command GetCommand(
          byte[] rawData,
          ref Resource.Picture.Command.PenType activePenType)
        {
          int num = (int) rawData[0];
          byte[] numArray = new byte[rawData.Length - 1];
          Array.Copy((Array) rawData, 1, (Array) numArray, 0, numArray.Length);
          switch (num)
          {
            case 240:
              return (Resource.Picture.Command) new Resource.Picture.Command.VisualColor(numArray);
            case 241:
              return (Resource.Picture.Command) new Resource.Picture.Command.DisableVisual(numArray);
            case 242:
              return (Resource.Picture.Command) new Resource.Picture.Command.PriorityColor(numArray);
            case 243:
              return (Resource.Picture.Command) new Resource.Picture.Command.DisablePriority(numArray);
            case 244:
              return numArray.Length == 0 ? (Resource.Picture.Command) null : (Resource.Picture.Command) new Resource.Picture.Command.YCorner(numArray);
            case 245:
              return numArray.Length == 0 ? (Resource.Picture.Command) null : (Resource.Picture.Command) new Resource.Picture.Command.XCorner(numArray);
            case 246:
              return numArray.Length == 0 ? (Resource.Picture.Command) null : (Resource.Picture.Command) new Resource.Picture.Command.AbsoluteLine(numArray);
            case 247:
              return numArray.Length == 0 ? (Resource.Picture.Command) null : (Resource.Picture.Command) new Resource.Picture.Command.RelativeLine(numArray);
            case 248:
              return numArray.Length == 0 ? (Resource.Picture.Command) null : (Resource.Picture.Command) new Resource.Picture.Command.Fill(numArray);
            case 249:
              return (Resource.Picture.Command) (activePenType = new Resource.Picture.Command.PenType(numArray));
            case 250:
              return (Resource.Picture.Command) new Resource.Picture.Command.Pen(numArray, activePenType);
            default:
              throw new Exception(string.Format("Unknown/unsupported command: 0x{0:X}!", (object) num));
          }
        }

        public virtual byte[] GetRawData() => (byte[]) null;

        public virtual Resource.Picture.Command[] Optimize() => (Resource.Picture.Command[]) null;

        public virtual void Draw(AGI.Drawing.Picture picture)
        {
        }

        public virtual void DrawHitPoints(Graphics graphics)
        {
        }

        public virtual void AddPoint(Point hitPoint) => this.PointList.Add((object) hitPoint);

        public virtual void UndoPoint() => this.PointList.RemoveAt(this.PointList.Count - 1);

        [Serializable]
        public class VisualColor : Resource.Picture.Command
        {
          public Color Color;

          public VisualColor(byte[] commandData)
          {
            if (commandData.Length != 1)
              throw new Exception(string.Format("Command has wrong number of data bytes. Expected 1, found {0}.", (object) commandData.Length));
            this.Color = commandData[0] <= (byte) 15 ? new Color(commandData[0]) : throw new Exception(string.Format("Color must be a value between 0 and 15, got {0}.", (object) commandData[0]));
          }

          public VisualColor(Color visualColor) => this.Color = visualColor;

          public override byte[] GetRawData() => new byte[2]
          {
            (byte) 240,
            this.Color.ColorIndex
          };

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            picture.VisualColor = this.Color;
            picture.VisualDraw = true;
          }
        }

        [Serializable]
        public class DisableVisual : Resource.Picture.Command
        {
          public DisableVisual(byte[] commandData)
          {
            if (commandData.Length != 0)
              throw new Exception(string.Format("Command has too many data bytes. Expected 0, found {0}.", (object) commandData.Length));
          }

          public DisableVisual()
          {
          }

          public override byte[] GetRawData() => new byte[1]
          {
            (byte) 241
          };

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            picture.VisualDraw = false;
          }
        }

        [Serializable]
        public class PriorityColor : Resource.Picture.Command
        {
          public Color Color;

          public PriorityColor(byte[] commandData)
          {
            if (commandData.Length != 1)
              throw new Exception(string.Format("Command has wrong number of data bytes. Expected 1, found {0}.", (object) commandData.Length));
            this.Color = commandData[0] <= (byte) 15 ? new Color(commandData[0]) : throw new Exception(string.Format("Color must be a value between 0 and 15, got {0}.", (object) commandData[0]));
          }

          public PriorityColor(Color priorityColor) => this.Color = priorityColor;

          public override byte[] GetRawData() => new byte[2]
          {
            (byte) 242,
            this.Color.ColorIndex
          };

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            picture.PriorityColor = this.Color;
            picture.PriorityDraw = true;
          }
        }

        [Serializable]
        public class DisablePriority : Resource.Picture.Command
        {
          public DisablePriority(byte[] commandData)
          {
            if (commandData.Length != 0)
              throw new Exception(string.Format("Command has too many data bytes. Expected 0, found {0}.", (object) commandData.Length));
          }

          public DisablePriority()
          {
          }

          public override byte[] GetRawData() => new byte[1]
          {
            (byte) 243
          };

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            picture.PriorityDraw = false;
          }
        }

        [Serializable]
        public class YCorner : Resource.Picture.Command
        {
          public YCorner(byte[] commandData)
          {
            if (commandData.Length < 2)
              throw new Exception(string.Format("Command must have at least two data bytes, found {0}.", (object) commandData.Length));
            int num1 = 0;
            try
            {
              byte[] numArray1 = commandData;
              int index1 = num1;
              int num2 = index1 + 1;
              int x = (int) numArray1[index1];
              byte[] numArray2 = commandData;
              int index2 = num2;
              int num3 = index2 + 1;
              int y = (int) numArray2[index2];
              Resource.Picture.ValidateXYData(x, y);
              this.PointList.Add((object) new Point(x, y));
              while (num3 < commandData.Length)
              {
                if (num3 % 2 == 0)
                  y = (int) commandData[num3++];
                else
                  x = (int) commandData[num3++];
                Resource.Picture.ValidateXYData(x, y);
                this.PointList.Add((object) new Point(x, y));
              }
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new Exception("Resource data corrupt!", (Exception) ex);
            }
          }

          public YCorner(Point startPoint) => this.AddPoint(startPoint);

          public override void AddPoint(Point hitPoint)
          {
            if (this.PointList.Count > 0)
            {
              Point point = (Point) this.PointList[this.PointList.Count - 1];
              if (point.X == hitPoint.X && point.Y != hitPoint.Y)
              {
                base.AddPoint(hitPoint);
              }
              else
              {
                if (point.Y != hitPoint.Y || point.X == hitPoint.X)
                  throw new Exception(string.Format("Point {0} is not valid in YCorner", (object) hitPoint));
                base.AddPoint(hitPoint);
              }
            }
            else
              base.AddPoint(hitPoint);
          }

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[3 + this.PointList.Count - 1];
            rawData[0] = (byte) 244;
            rawData[1] = (byte) ((Point) this.PointList[0]).X;
            rawData[2] = (byte) ((Point) this.PointList[0]).Y;
            int index = 3;
            foreach (Point point in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              rawData[index] = index % 2 != 0 ? (byte) point.Y : (byte) point.X;
              ++index;
            }
            return rawData;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            Point point = (Point) this.PointList[0];
            picture.Plot(point);
            foreach (Point end in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              picture.Line(point, end);
              point = end;
            }
          }
        }

        [Serializable]
        public class XCorner : Resource.Picture.Command
        {
          public XCorner(byte[] commandData)
          {
            if (commandData.Length < 2)
              throw new Exception(string.Format("Command must have at least two data bytes, found {0}.", (object) commandData.Length));
            int num1 = 0;
            try
            {
              byte[] numArray1 = commandData;
              int index1 = num1;
              int num2 = index1 + 1;
              int x = (int) numArray1[index1];
              byte[] numArray2 = commandData;
              int index2 = num2;
              int num3 = index2 + 1;
              int y = (int) numArray2[index2];
              Resource.Picture.ValidateXYData(x, y);
              this.PointList.Add((object) new Point(x, y));
              while (num3 < commandData.Length)
              {
                if (num3 % 2 == 0)
                  x = (int) commandData[num3++];
                else
                  y = (int) commandData[num3++];
                Resource.Picture.ValidateXYData(x, y);
                this.PointList.Add((object) new Point(x, y));
              }
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new Exception("Resource data corrupt!", (Exception) ex);
            }
          }

          public XCorner(Point startPoint) => this.AddPoint(startPoint);

          public override void AddPoint(Point hitPoint)
          {
            if (this.PointList.Count > 0)
            {
              Point point = (Point) this.PointList[this.PointList.Count - 1];
              if (point.X == hitPoint.X && point.Y != hitPoint.Y)
              {
                base.AddPoint(hitPoint);
              }
              else
              {
                if (point.Y != hitPoint.Y || point.X == hitPoint.X)
                  throw new Exception(string.Format("Point {0} is not valid in XCorner", (object) hitPoint));
                base.AddPoint(hitPoint);
              }
            }
            else
              base.AddPoint(hitPoint);
          }

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[3 + this.PointList.Count - 1];
            rawData[0] = (byte) 245;
            rawData[1] = (byte) ((Point) this.PointList[0]).X;
            rawData[2] = (byte) ((Point) this.PointList[0]).Y;
            int index = 3;
            foreach (Point point in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              rawData[index] = index % 2 != 0 ? (byte) point.X : (byte) point.Y;
              ++index;
            }
            return rawData;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            Point point = (Point) this.PointList[0];
            picture.Plot(point);
            foreach (Point end in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              picture.Line(point, end);
              point = end;
            }
          }
        }

        [Serializable]
        public class AbsoluteLine : Resource.Picture.Command
        {
          public AbsoluteLine(byte[] commandData)
          {
            if (commandData.Length % 2 != 0)
              throw new Exception("Command has a wrong number of data bytes.");
            if (commandData.Length < 2)
              throw new Exception(string.Format("Command must have at least two data bytes, found {0}.", (object) commandData.Length));
            try
            {
              int num1 = 0;
              while (num1 < commandData.Length)
              {
                byte[] numArray1 = commandData;
                int index1 = num1;
                int num2 = index1 + 1;
                int x = (int) numArray1[index1];
                byte[] numArray2 = commandData;
                int index2 = num2;
                num1 = index2 + 1;
                int y = (int) numArray2[index2];
                Resource.Picture.ValidateXYData(x, y);
                this.AddPoint(new Point(x, y));
              }
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new Exception("Resource data corrupt!", (Exception) ex);
            }
          }

          public AbsoluteLine(Point startPoint) => this.AddPoint(startPoint);

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[1 + this.PointList.Count * 2];
            rawData[0] = (byte) 246;
            int index = 1;
            foreach (Point point in this.PointList)
            {
              rawData[index] = (byte) point.X;
              rawData[index + 1] = (byte) point.Y;
              index += 2;
            }
            return rawData;
          }

          public override Resource.Picture.Command[] Optimize()
          {
            for (int index = 0; index < this.PointList.Count - 1; ++index)
            {
              if ((Point) this.PointList[index] == (Point) this.PointList[index + 1])
                this.PointList.RemoveAt(index);
            }
            int[] numArray1 = new int[3];
            int[] numArray2 = new int[3];
            for (int index = 0; index < this.PointList.Count - 1; ++index)
            {
              Point point1 = (Point) this.PointList[index];
              Point point2 = (Point) this.PointList[index + 1];
              if (numArray1[0] == 0)
              {
                if (point1.Y == point2.Y)
                {
                  numArray1[1] = index;
                  numArray1[2] = index % 2;
                }
                if (point1.X == point2.X)
                {
                  numArray1[1] = index;
                  numArray1[2] = index % 2 == 0 ? 1 : 0;
                }
              }
              if (index % 2 == numArray1[2])
              {
                if (point1.Y == point2.Y)
                {
                  ++numArray1[0];
                }
                else
                {
                  if (numArray1[0] > numArray2[0])
                    numArray1.CopyTo((Array) numArray2, 0);
                  numArray1[0] = 0;
                }
              }
              else if (point1.X == point2.X)
              {
                ++numArray1[0];
              }
              else
              {
                if (numArray1[0] > numArray2[0])
                  numArray1.CopyTo((Array) numArray2, 0);
                numArray1[0] = 0;
              }
            }
            if (numArray1[0] > numArray2[0])
              numArray1.CopyTo((Array) numArray2, 0);
            if (numArray2[0] > 0)
            {
              int num1 = 1 + this.PointList.Count * 2;
              int num2 = 0;
              ArrayList arrayList = new ArrayList();
              if (numArray2[1] > 0)
              {
                Resource.Picture.Command.AbsoluteLine absoluteLine = new Resource.Picture.Command.AbsoluteLine((Point) this.PointList[0]);
                foreach (Point hitPoint in this.PointList.GetRange(1, numArray2[1]))
                  absoluteLine.AddPoint(hitPoint);
                Resource.Picture.Command[] commandArray = absoluteLine.Optimize();
                if (commandArray == null)
                {
                  num2 += absoluteLine.GetRawData().Length;
                  arrayList.Add((object) absoluteLine);
                }
                else
                {
                  foreach (Resource.Picture.Command command in commandArray)
                  {
                    num2 += command.GetRawData().Length;
                    arrayList.Add((object) command);
                  }
                }
              }
              if (numArray2[1] + numArray2[0] < this.PointList.Count - 1)
              {
                int index = numArray2[1] + numArray2[0];
                Resource.Picture.Command.AbsoluteLine absoluteLine = new Resource.Picture.Command.AbsoluteLine((Point) this.PointList[index]);
                foreach (Point hitPoint in this.PointList.GetRange(index + 1, this.PointList.Count - index - 1))
                  absoluteLine.AddPoint(hitPoint);
                Resource.Picture.Command[] commandArray = absoluteLine.Optimize();
                if (commandArray == null)
                {
                  num2 += absoluteLine.GetRawData().Length;
                  arrayList.Add((object) absoluteLine);
                }
                else
                {
                  foreach (Resource.Picture.Command command in commandArray)
                  {
                    num2 += command.GetRawData().Length;
                    arrayList.Add((object) command);
                  }
                }
              }
              Resource.Picture.Command command1 = numArray2[2] != numArray2[1] % 2 ? (Resource.Picture.Command) new Resource.Picture.Command.YCorner((Point) this.PointList[numArray2[1]]) : (Resource.Picture.Command) new Resource.Picture.Command.XCorner((Point) this.PointList[numArray2[1]]);
              foreach (Point hitPoint in this.PointList.GetRange(numArray2[1] + 1, numArray2[0]))
                command1.AddPoint(hitPoint);
              int num3 = num2 + command1.GetRawData().Length;
              arrayList.Add((object) command1);
              if (num3 < num1)
              {
                Resource.Picture.Command[] commandArray = new Resource.Picture.Command[arrayList.Count];
                for (int index = 0; index < arrayList.Count; ++index)
                  commandArray[index] = (Resource.Picture.Command) arrayList[index];
                return commandArray;
              }
            }
            numArray1[0] = numArray1[1] = 0;
            numArray2[0] = numArray1[1] = 0;
            for (int index = 0; index < this.PointList.Count - 1; ++index)
            {
              Point point3 = (Point) this.PointList[index];
              Point point4 = (Point) this.PointList[index + 1];
              if (Math.Abs(point4.X - point3.X) > 7 || Math.Abs(point4.Y - point3.Y) > 7)
              {
                if (numArray1[0] > numArray2[0])
                  numArray1.CopyTo((Array) numArray2, 0);
                numArray1[0] = 0;
                numArray1[1] = index + 1;
              }
              else
                ++numArray1[0];
            }
            if (numArray1[0] > numArray2[0])
              numArray1.CopyTo((Array) numArray2, 0);
            if (numArray2[0] > 0)
            {
              int num4 = 1 + this.PointList.Count * 2;
              int num5 = 0;
              ArrayList arrayList = new ArrayList();
              if (numArray2[1] > 0)
              {
                Resource.Picture.Command.AbsoluteLine absoluteLine = new Resource.Picture.Command.AbsoluteLine((Point) this.PointList[0]);
                foreach (Point hitPoint in this.PointList.GetRange(1, numArray2[1]))
                  absoluteLine.AddPoint(hitPoint);
                Resource.Picture.Command[] commandArray = absoluteLine.Optimize();
                if (commandArray == null)
                {
                  num5 += absoluteLine.GetRawData().Length;
                  arrayList.Add((object) absoluteLine);
                }
                else
                {
                  foreach (Resource.Picture.Command command in commandArray)
                  {
                    num5 += command.GetRawData().Length;
                    arrayList.Add((object) command);
                  }
                }
              }
              if (numArray2[1] + numArray2[0] < this.PointList.Count - 1)
              {
                int index = numArray2[1] + numArray2[0];
                Resource.Picture.Command.AbsoluteLine absoluteLine = new Resource.Picture.Command.AbsoluteLine((Point) this.PointList[index]);
                foreach (Point hitPoint in this.PointList.GetRange(index + 1, this.PointList.Count - index - 1))
                  absoluteLine.AddPoint(hitPoint);
                Resource.Picture.Command[] commandArray = absoluteLine.Optimize();
                if (commandArray == null)
                {
                  num5 += absoluteLine.GetRawData().Length;
                  arrayList.Add((object) absoluteLine);
                }
                else
                {
                  foreach (Resource.Picture.Command command in commandArray)
                  {
                    num5 += command.GetRawData().Length;
                    arrayList.Add((object) command);
                  }
                }
              }
              Resource.Picture.Command.RelativeLine relativeLine = new Resource.Picture.Command.RelativeLine((Point) this.PointList[numArray2[1]]);
              foreach (Point hitPoint in this.PointList.GetRange(numArray2[1] + 1, numArray2[0]))
                relativeLine.AddPoint(hitPoint);
              int num6 = num5 + relativeLine.GetRawData().Length;
              arrayList.Add((object) relativeLine);
              if (num6 < num4)
              {
                Resource.Picture.Command[] commandArray = new Resource.Picture.Command[arrayList.Count];
                for (int index = 0; index < arrayList.Count; ++index)
                  commandArray[index] = (Resource.Picture.Command) arrayList[index];
                return commandArray;
              }
            }
            return (Resource.Picture.Command[]) null;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            Point point = (Point) this.PointList[0];
            picture.Plot(point);
            foreach (Point end in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              picture.Line(point, end);
              point = end;
            }
          }
        }

        [Serializable]
        public class RelativeLine : Resource.Picture.Command
        {
          public RelativeLine(byte[] commandData)
          {
            if (commandData.Length < 2)
              throw new Exception(string.Format("Command must have at least two data bytes, found {0}.", (object) commandData.Length));
            try
            {
              int num1 = 0;
              byte[] numArray1 = commandData;
              int index1 = num1;
              int num2 = index1 + 1;
              int x = (int) numArray1[index1];
              byte[] numArray2 = commandData;
              int index2 = num2;
              int num3 = index2 + 1;
              int y = (int) numArray2[index2];
              Resource.Picture.ValidateXYData(x, y);
              this.PointList.Add((object) new Point(x, y));
              while (num3 < commandData.Length)
              {
                byte num4 = commandData[num3++];
                int num5 = (((int) num4 & 112) >> 4) * (((int) num4 & 128) == 128 ? -1 : 1);
                int num6 = ((int) num4 & 7) * (((int) num4 & 8) == 8 ? -1 : 1);
                x += num5;
                y += num6;
                this.PointList.Add((object) new Point(x, y));
              }
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new Exception("Resource data corrupt!", (Exception) ex);
            }
          }

          public RelativeLine(Point startPoint) => this.AddPoint(startPoint);

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[3 + this.PointList.Count - 1];
            rawData[0] = (byte) 247;
            Point point1 = (Point) this.PointList[0];
            rawData[1] = (byte) point1.X;
            rawData[2] = (byte) point1.Y;
            int num1 = 3;
            foreach (Point point2 in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              int num2 = point2.X - point1.X;
              int num3 = point2.Y - point1.Y;
              byte num4 = 0;
              if (num2 < 0)
              {
                num4 += (byte) 128;
                num2 *= -1;
              }
              if (num3 < 0)
              {
                num4 += (byte) 8;
                num3 *= -1;
              }
              if (num2 > 7 || num3 > 7)
                throw new Exception(string.Format("RelativeLine: Point {0} causes offset overflow (xOffset: {1} yOffset: {2})", (object) point2, (object) num2, (object) num3));
              byte num5 = (byte) ((uint) (byte) ((uint) num4 + (uint) (byte) (num2 << 4)) + (uint) (byte) num3);
              rawData[num1++] = num5;
              point1 = point2;
            }
            return rawData;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            Point point = (Point) this.PointList[0];
            picture.Plot(point);
            foreach (Point end in this.PointList.GetRange(1, this.PointList.Count - 1))
            {
              picture.Line(point, end);
              point = end;
            }
          }
        }

        [Serializable]
        public class Fill : Resource.Picture.Command
        {
          public Fill(byte[] commandData)
          {
            if (commandData.Length % 2 != 0)
              throw new Exception("Command has a wrong number of data bytes.");
            try
            {
              int num1 = 0;
              while (num1 < commandData.Length)
              {
                byte[] numArray1 = commandData;
                int index1 = num1;
                int num2 = index1 + 1;
                int x = (int) numArray1[index1];
                byte[] numArray2 = commandData;
                int index2 = num2;
                num1 = index2 + 1;
                int y = (int) numArray2[index2];
                Resource.Picture.ValidateXYData(x, y);
                this.AddPoint(new Point(x, y));
              }
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new Exception("Resource data corrupt!", (Exception) ex);
            }
          }

          public Fill(Point hitPoint) => this.AddPoint(hitPoint);

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[1 + this.PointList.Count * 2];
            rawData[0] = (byte) 248;
            int index = 1;
            foreach (Point point in this.PointList)
            {
              rawData[index] = (byte) point.X;
              rawData[index + 1] = (byte) point.Y;
              index += 2;
            }
            return rawData;
          }

          public override Resource.Picture.Command[] Optimize()
          {
            for (int index1 = 0; index1 < this.PointList.Count; ++index1)
            {
              for (int index2 = index1 + 1; index2 < this.PointList.Count; ++index2)
              {
                if ((Point) this.PointList[index1] == (Point) this.PointList[index2])
                  this.PointList.RemoveAt(index2);
              }
            }
            return (Resource.Picture.Command[]) null;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            foreach (Point point in this.PointList)
              picture.Fill(point);
          }
        }

        [Serializable]
        public class PenType : Resource.Picture.Command
        {
          public byte Size;
          public bool IsRectangle;
          public bool IsSplatter;

          public PenType()
          {
            this.Size = (byte) 0;
            this.IsSplatter = false;
            this.IsRectangle = false;
          }

          public PenType(byte[] commandData)
          {
            this.Size = (byte) ((uint) commandData[0] & 7U);
            this.IsRectangle = ((int) commandData[0] & 16) == 16;
            this.IsSplatter = ((int) commandData[0] & 32) == 32;
          }

          public PenType(byte size, bool isRectangle, bool isSplatter)
          {
            this.Size = size;
            this.IsRectangle = isRectangle;
            this.IsSplatter = isSplatter;
          }

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[2]
            {
              (byte) 249,
              (byte) 0
            };
            byte size = this.Size;
            if (this.IsRectangle)
              size += (byte) 16;
            if (this.IsSplatter)
              size += (byte) 32;
            rawData[1] = size;
            return rawData;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            picture.PenSize = this.Size;
            picture.PenIsRectangle = this.IsRectangle;
            picture.PenIsSplatter = this.IsSplatter;
          }
        }

        [Serializable]
        public class Pen : Resource.Picture.Command
        {
          public Resource.Picture.Command.PenType Type;
          public ArrayList PatternList;
          private byte[] buffer;

          public Pen(byte[] commandData, Resource.Picture.Command.PenType type)
          {
            this.Type = type != null ? type : new Resource.Picture.Command.PenType();
            this.PatternList = new ArrayList();
            if (this.Type.IsSplatter)
            {
              if (commandData.Length % 3 != 0)
                throw new Exception("Command has a wrong number of data bytes.");
            }
            else if (commandData.Length % 2 != 0)
              throw new Exception("Command has a wrong number of data bytes.");
            try
            {
              int num1 = 0;
              while (num1 < commandData.Length)
              {
                if (this.Type.IsSplatter)
                  this.PatternList.Add((object) commandData[num1++]);
                byte[] numArray1 = commandData;
                int index1 = num1;
                int num2 = index1 + 1;
                int x = (int) numArray1[index1];
                byte[] numArray2 = commandData;
                int index2 = num2;
                num1 = index2 + 1;
                int y = (int) numArray2[index2];
                this.AddPoint(new Point(x, y));
              }
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new Exception("Resource data corrupt!", (Exception) ex);
            }
            this.buffer = (byte[]) commandData.Clone();
          }

          public override byte[] GetRawData()
          {
            byte[] rawData = new byte[1 + this.PatternList.Count + this.PointList.Count * 2];
            rawData[0] = (byte) 250;
            int num1 = 1;
            int num2 = 0;
            foreach (Point point in this.PointList)
            {
              if (this.Type.IsSplatter)
                rawData[num1++] = (byte) this.PatternList[num2++];
              byte[] numArray1 = rawData;
              int index1 = num1;
              int num3 = index1 + 1;
              int x = (int) (byte) point.X;
              numArray1[index1] = (byte) x;
              byte[] numArray2 = rawData;
              int index2 = num3;
              num1 = index2 + 1;
              int y = (int) (byte) point.Y;
              numArray2[index2] = (byte) y;
            }
            return rawData;
          }

          public override void Draw(AGI.Drawing.Picture picture)
          {
            base.Draw(picture);
            int num = 0;
            foreach (Point point in this.PointList)
              picture.PlotPattern(this.Type.IsSplatter ? (int) (byte) this.PatternList[num++] : 0, point);
          }
        }
      }
    }

    public class View : Resource
    {
      public ArrayList Loops;
      public string Description;

      public View(byte[] rawData)
      {
        this.Loops = new ArrayList();
        this.Decode(rawData);
      }

      public View()
      {
        this.Loops = new ArrayList();
        this.Description = "";
      }

      public override byte[] Encode()
      {
        MemoryStream memoryStream = new MemoryStream();
        memoryStream.WriteByte((byte) 1);
        memoryStream.WriteByte((byte) 1);
        memoryStream.WriteByte((byte) this.Loops.Count);
        byte[] buffer1 = new byte[2 + this.Loops.Count * 2];
        buffer1[0] = buffer1[1] = (byte) 0;
        memoryStream.Write(buffer1, 0, buffer1.Length);
        for (int index = 0; index < this.Loops.Count; ++index)
        {
          Resource.View.Loop loop = (Resource.View.Loop) this.Loops[index];
          buffer1[2 + index * 2] = (byte) ((ulong) memoryStream.Position & (ulong) byte.MaxValue);
          buffer1[2 + index * 2 + 1] = (byte) ((memoryStream.Position & 65280L) >> 8);
          byte[] buffer2 = loop.Encode();
          memoryStream.Write(buffer2, 0, buffer2.Length);
        }
        if (this.Description != "")
        {
          buffer1[0] = (byte) ((ulong) memoryStream.Position & (ulong) byte.MaxValue);
          buffer1[1] = (byte) ((memoryStream.Position & 65280L) >> 8);
          byte[] bytes = Encoding.ASCII.GetBytes(this.Description);
          memoryStream.Write(bytes, 0, bytes.Length);
        }
        memoryStream.Position = 3L;
        memoryStream.Write(buffer1, 0, buffer1.Length);
        memoryStream.Capacity = (int) memoryStream.Length;
        return memoryStream.GetBuffer();
      }

      protected void Decode(byte[] rawData)
      {
        if (rawData == null)
          throw new ArgumentNullException(nameof (rawData), "The data to decode cannot be null.");
        int num = rawData.Length >= 5 ? (int) rawData[2] : throw new ArgumentException("There is not enough data to decode!", nameof (rawData));
        int index1 = (int) rawData[3] + ((int) rawData[4] << 8);
        if (index1 == 0)
        {
          this.Description = "";
        }
        else
        {
          int index2 = index1;
          while (index2 < rawData.Length && rawData[index2] != (byte) 0)
            ++index2;
          this.Description = Encoding.ASCII.GetString(rawData, index1, index2 - index1);
        }
        if (num == 0)
          return;
        int index3 = 5;
        ArrayList arrayList = new ArrayList();
        for (int index4 = 0; index4 < num; ++index4)
        {
          if (index3 + 2 >= rawData.Length)
            throw new Exception("End of data found while decoding!");
          int sourceIndex = (int) rawData[index3] + ((int) rawData[index3 + 1] << 8);
          index3 += 2;
          if (sourceIndex >= rawData.Length)
            throw new Exception("Loop position is out of bounds!");
          if (arrayList.Contains((object) sourceIndex))
          {
            this.Loops.Add(this.Loops[arrayList.IndexOf((object) sourceIndex)]);
            arrayList.Add((object) sourceIndex);
          }
          else
          {
            arrayList.Add((object) sourceIndex);
            byte[] numArray = new byte[rawData.Length - sourceIndex];
            Array.Copy((Array) rawData, sourceIndex, (Array) numArray, 0, numArray.Length);
            this.Loops.Add((object) new Resource.View.Loop(numArray));
          }
        }
        this.IsModified = false;
      }

      public int InsertLoop(int insertPosition)
      {
        Resource.View.Loop loop = new Resource.View.Loop();
        loop.AddCel(6, 32);
        this.Loops.Insert(insertPosition, (object) loop);
        return insertPosition;
      }

      public int AddLoop()
      {
        Resource.View.Loop loop = new Resource.View.Loop();
        loop.AddCel(6, 32);
        this.Loops.Add((object) loop);
        return this.Loops.Count - 1;
      }

      public Resource.View.Loop GetLoop(int loopIndex) => loopIndex < 0 || loopIndex >= this.Loops.Count ? (Resource.View.Loop) null : (Resource.View.Loop) this.Loops[loopIndex];

      public int DeleteLoop(int loopIndex)
      {
        this.Loops.RemoveAt(loopIndex);
        return this.Loops.Count;
      }

      public class Loop
      {
        public ArrayList Cels;

        public Loop(byte[] loopData)
        {
          this.Cels = new ArrayList();
          int num = (int) loopData[0];
          if (num == 0)
            return;
          int index1 = 1;
          for (int index2 = 0; index2 < num; ++index2)
          {
            int sourceIndex = (int) loopData[index1] + ((int) loopData[index1 + 1] << 8);
            index1 += 2;
            byte[] numArray = new byte[(index2 == num - 1 ? loopData.Length : (int) loopData[index1] + ((int) loopData[index1 + 1] << 8)) - sourceIndex];
            Array.Copy((Array) loopData, sourceIndex, (Array) numArray, 0, numArray.Length);
            this.Cels.Add((object) new Resource.View.Cel(numArray));
          }
        }

        public Loop() => this.Cels = new ArrayList();

        public byte[] Encode()
        {
          MemoryStream memoryStream = new MemoryStream();
          memoryStream.WriteByte((byte) this.Cels.Count);
          byte[] buffer1 = new byte[this.Cels.Count * 2];
          memoryStream.Write(buffer1, 0, buffer1.Length);
          for (int index = 0; index < this.Cels.Count; ++index)
          {
            Resource.View.Cel cel = (Resource.View.Cel) this.Cels[index];
            buffer1[index * 2] = (byte) ((ulong) memoryStream.Position & (ulong) byte.MaxValue);
            buffer1[index * 2 + 1] = (byte) ((memoryStream.Position & 65280L) >> 8);
            byte[] buffer2 = cel.Encode();
            memoryStream.Write(buffer2, 0, buffer2.Length);
          }
          memoryStream.Position = 1L;
          memoryStream.Write(buffer1, 0, buffer1.Length);
          memoryStream.Capacity = (int) memoryStream.Length;
          return memoryStream.GetBuffer();
        }

        public int InsertCel(int insertPosition, int width, int height)
        {
          Resource.View.Cel cel = new Resource.View.Cel(width, height);
          this.Cels.Insert(insertPosition, (object) cel);
          return insertPosition;
        }

        public int AddCel(int width, int height)
        {
          this.Cels.Add((object) new Resource.View.Cel(width, height));
          return this.Cels.Count - 1;
        }

        public Resource.View.Cel GetCel(int celIndex) => celIndex < 0 || celIndex >= this.Cels.Count ? (Resource.View.Cel) null : (Resource.View.Cel) this.Cels[celIndex];

        public int DeleteCel(int celIndex)
        {
          this.Cels.RemoveAt(celIndex);
          return this.Cels.Count;
        }
      }

      public class Cel
      {
        public AGI.Drawing.View Screen;
        public Color TransparentColor;
        public bool IsMirrored;
        public int MirrorOf;

        public Cel(byte[] celData)
        {
          int width = (int) celData[0];
          int height = (int) celData[1];
          this.IsMirrored = ((int) celData[2] & 128) == 128;
          this.MirrorOf = ((int) celData[2] & 112) >> 4;
          this.TransparentColor = new Color((byte) ((uint) celData[2] & 15U));
          this.Screen = new AGI.Drawing.View(new Size(width, height));
          int x = 0;
          int y = 0;
          this.Screen.BeginDraw();
          for (int index1 = 3; index1 < celData.Length; ++index1)
          {
            if (celData[index1] == (byte) 0)
            {
              Color transparentColor = this.TransparentColor;
              for (; x < width; ++x)
                this.Screen.Plot(x, y, transparentColor);
              x = 0;
              ++y;
              if (y >= height)
                break;
            }
            else
            {
              Color color = (Color) (byte) (((int) celData[index1] & 240) >> 4);
              for (int index2 = 0; index2 < ((int) celData[index1] & 15); ++index2)
                this.Screen.Plot(x++, y, color);
            }
          }
          this.Screen.EndDraw();
        }

        public Cel(int width, int height)
        {
          this.Screen = new AGI.Drawing.View(new Size(width, height));
          this.TransparentColor = Color.Black;
          this.IsMirrored = false;
          this.MirrorOf = 0;
        }

        public byte[] Encode()
        {
          MemoryStream memoryStream = new MemoryStream();
          memoryStream.WriteByte((byte) this.Screen.Bitmap.Width);
          memoryStream.WriteByte((byte) this.Screen.Bitmap.Height);
          byte num1 = 0;
          if (this.IsMirrored)
            num1 = (byte) ((uint) (byte) ((uint) num1 + 128U) + (uint) (byte) ((this.MirrorOf & 7) << 4));
          byte num2 = (byte) ((uint) num1 + (uint) (byte) ((uint) (byte) this.TransparentColor & 15U));
          memoryStream.WriteByte(num2);
          this.Screen.BeginDraw();
          for (int y = 0; y < this.Screen.Bitmap.Height; ++y)
          {
            Color color = this.TransparentColor;
            int num3 = 0;
            for (int x = 0; x < this.Screen.Bitmap.Width; ++x)
            {
              if (this.Screen.GetPixel(x, y) == color)
              {
                ++num3;
              }
              else
              {
                byte num4;
                for (; num3 > 0; num3 -= (int) num4 & 15)
                {
                  num4 = num3 > 15 ? (byte) 15 : (byte) num3;
                  byte num5 = (byte) ((uint) (byte) (((int) (byte) color & 15) << 4) + (uint) (byte) ((uint) num4 & 15U));
                  memoryStream.WriteByte(num5);
                }
                color = this.Screen.GetPixel(x, y);
                num3 = 1;
              }
            }
            byte num6;
            if (color != this.TransparentColor)
            {
              for (; num3 > 0; num3 -= (int) num6 & 15)
              {
                num6 = num3 > 15 ? (byte) 15 : (byte) num3;
                byte num7 = (byte) ((uint) (byte) (((int) (byte) color & 15) << 4) + (uint) (byte) ((uint) num6 & 15U));
                memoryStream.WriteByte(num7);
              }
            }
            memoryStream.WriteByte((byte) 0);
          }
          this.Screen.EndDraw();
          memoryStream.Capacity = (int) memoryStream.Length;
          return memoryStream.GetBuffer();
        }

        public void Flip() => this.Screen.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
      }
    }

    public class Sound : Resource
    {
      public List<List<Resource.Sound.Note>> Notes;
      private byte[] rawData;

      public Sound(byte[] rawData)
      {
        this.Notes = new List<List<Resource.Sound.Note>>();
        for (int index = 0; index < 4; ++index)
          this.Notes.Add(new List<Resource.Sound.Note>());
        this.Decode(rawData);
      }

      public override byte[] Encode() => this.rawData;

      public void Decode(byte[] rawData)
      {
        for (int index1 = 0; index1 < 4; ++index1)
        {
          int num1 = (int) rawData[index1 * 2] | (int) rawData[index1 * 2 + 1] << 8;
          int num2 = index1 < 3 ? ((int) rawData[index1 * 2 + 2] | (int) rawData[index1 * 2 + 3] << 8) - 5 : rawData.Length;
          for (int index2 = num1; index2 < num2; index2 += 5)
          {
            Resource.Sound.Note note = new Resource.Sound.Note(index1);
            byte[] rawData1 = new byte[5]
            {
              index2 < rawData.Length ? rawData[index2] : (byte) 0,
              index2 + 1 < rawData.Length ? rawData[index2 + 1] : (byte) 0,
              index2 + 2 < rawData.Length ? rawData[index2 + 2] : (byte) 0,
              index2 + 3 < rawData.Length ? rawData[index2 + 3] : (byte) 0,
              index2 + 4 < rawData.Length ? rawData[index2 + 4] : (byte) 0
            };
            if (note.Decode(rawData1))
              this.Notes[index1].Add(note);
          }
        }
      }

      public class Note
      {
        public int voiceNum;
        public int duration;
        public double frequency;
        public int volume;
        public int origVolume;
        public int frequencyCount;
        public byte[] rawData;

        public Note(int voiceNum) => this.voiceNum = voiceNum;

        public bool Decode(byte[] rawData)
        {
          int num = (int) rawData[0] | (int) rawData[1] << 8;
          if (num == (int) ushort.MaxValue)
            return false;
          this.duration = num;
          this.frequencyCount = (((int) rawData[2] & 63) << 4) + ((int) rawData[3] & 15);
          this.origVolume = (int) rawData[4] & 15;
          this.volume = 8;
          this.frequency = this.frequencyCount > 0 ? 111860.0 / (double) this.frequencyCount : 0.0;
          this.rawData = rawData;
          return true;
        }

        public byte[] Encode()
        {
          byte[] numArray = new byte[5];
          int num = this.frequency == 0.0 ? 0 : (int) (111860.0 / this.frequency);
          numArray[0] = (byte) (this.duration & (int) byte.MaxValue);
          numArray[1] = (byte) (this.duration >> 8 & (int) byte.MaxValue);
          numArray[2] = (byte) (num >> 4 & 63);
          numArray[3] = (byte) (128 | this.voiceNum << 5 & 96 | num & 15);
          numArray[4] = (byte) (144 | this.voiceNum << 5 & 96 | this.volume & 15);
          this.rawData = numArray;
          return numArray;
        }
      }
    }

    public class Logic : Resource
    {
      private bool messagesCrypted;
      private static Resource.Logic.Operation[] TEST_OPERATIONS = new Resource.Logic.Operation[19]
      {
        null,
        new Resource.Logic.Operation(1, "equaln(VAR,NUM)", "ExpressionEqual"),
        new Resource.Logic.Operation(2, "equalv(VAR,VAR)", "ExpressionEqualV"),
        new Resource.Logic.Operation(3, "lessn(VAR,NUM)", "ExpressionLess"),
        new Resource.Logic.Operation(4, "lessv(VAR,VAR)", "ExpressionLessV"),
        new Resource.Logic.Operation(5, "greatern(VAR,NUM)", "ExpressionGreater"),
        new Resource.Logic.Operation(6, "greaterv(VAR,VAR)", "ExpressionGreaterV"),
        new Resource.Logic.Operation(7, "isset(FLAG)", "ExpressionIsSet"),
        new Resource.Logic.Operation(8, "isset.v(VAR)", "ExpressionIsSetV"),
        new Resource.Logic.Operation(9, "has(OBJECT)", "ExpressionHas"),
        new Resource.Logic.Operation(10, "obj.in.room(OBJECT,VAR)", "ExpressionObjInRoom"),
        new Resource.Logic.Operation(11, "posn(OBJECT,NUM,NUM,NUM,NUM)", "ExpressionPosN"),
        new Resource.Logic.Operation(12, "controller(NUM)", "ExpressionController"),
        new Resource.Logic.Operation(13, "have.key()", "ExpressionHaveKey"),
        new Resource.Logic.Operation(14, "said(WORDLIST)", "ExpressionSaid"),
        new Resource.Logic.Operation(15, "compare.strings(NUM,NUM)", "ExpressionStringCompare"),
        new Resource.Logic.Operation(16, "obj.in.box(OBJECT,NUM,NUM,NUM,NUM)", "ExpressionObjInBox"),
        new Resource.Logic.Operation(17, "center.posn(OBJECT,NUM,NUM,NUM,NUM)", "ExpressionCentrePosition"),
        new Resource.Logic.Operation(18, "right.posn(OBJECT,NUM,NUM,NUM,NUM)", "ExpressionRightPosition")
      };
      private static Resource.Logic.Operation[] ACTION_OPERATIONS = new Resource.Logic.Operation[183]
      {
        new Resource.Logic.Operation(0, "return()", "InstructionReturn"),
        new Resource.Logic.Operation(1, "increment(VAR)", "InstructionIncrement"),
        new Resource.Logic.Operation(2, "decrement(VAR)", "InstructionDecrement"),
        new Resource.Logic.Operation(3, "assignn(VAR,NUM)", "InstructionAssign"),
        new Resource.Logic.Operation(4, "assignv(VAR,VAR)", "InstructionAssignV"),
        new Resource.Logic.Operation(5, "addn(VAR,NUM)", "InstructionAdd"),
        new Resource.Logic.Operation(6, "addv(VAR,VAR)", "InstructionAddV"),
        new Resource.Logic.Operation(7, "subn(VAR,NUM)", "InstructionSubstract"),
        new Resource.Logic.Operation(8, "subv(VAR,VAR)", "InstructionSubstractV"),
        new Resource.Logic.Operation(9, "lindirectv(VAR,VAR)", "InstructionIndirect"),
        new Resource.Logic.Operation(10, "rindirect(VAR,VAR)", "InstructionIndirect"),
        new Resource.Logic.Operation(11, "lindirectn(VAR,NUM)", "InstructionIndirect"),
        new Resource.Logic.Operation(12, "set(FLAG)", "InstructionSet"),
        new Resource.Logic.Operation(13, "reset(FLAG)", "InstructionReset"),
        new Resource.Logic.Operation(14, "toggle(FLAG)", "InstructionToggle"),
        new Resource.Logic.Operation(15, "set.v(VAR)", "InstructionSet"),
        new Resource.Logic.Operation(16, "reset.v(VAR)", "InstructionReset"),
        new Resource.Logic.Operation(17, "toggle.v(VAR)", "InstructionToggle"),
        new Resource.Logic.Operation(18, "new.room(NUM)", "InstructionNewRoom"),
        new Resource.Logic.Operation(19, "new.room.f(VAR)", "InstructionNewRoomV"),
        new Resource.Logic.Operation(20, "load.logics(NUM)", "InstructionLoadLogic"),
        new Resource.Logic.Operation(21, "load.logics.f(VAR)", "InstructionLoadLogicV"),
        new Resource.Logic.Operation(22, "call(NUM)", "InstructionCall"),
        new Resource.Logic.Operation(23, "call.f(VAR)", "InstructionCallV"),
        new Resource.Logic.Operation(24, "load.pic(VAR)", "InstructionLoadPic"),
        new Resource.Logic.Operation(25, "draw.pic(VAR)", "InstructionDrawPic"),
        new Resource.Logic.Operation(26, "show.pic()", "InstructionShowPic"),
        new Resource.Logic.Operation(27, "discard.pic(VAR)", "InstructionDiscardPic"),
        new Resource.Logic.Operation(28, "overlay.pic(VAR)", "InstructionOverlayPic"),
        new Resource.Logic.Operation(29, "show.pri.screen()", "InstructionShowPriScreen"),
        new Resource.Logic.Operation(30, "load.view(VIEW)", "InstructionLoadView"),
        new Resource.Logic.Operation(31, "load.view.f(VAR)", "InstructionLoadViewV"),
        new Resource.Logic.Operation(32, "discard.view(VIEW)", "InstructionDiscardView"),
        new Resource.Logic.Operation(33, "animate.obj(OBJECT)", "InstructionAnimateObject"),
        new Resource.Logic.Operation(34, "unanimate.all()", "InstructionUnanimateAll"),
        new Resource.Logic.Operation(35, "draw(OBJECT)", "InstructionDraw"),
        new Resource.Logic.Operation(36, "erase(OBJECT)", "InstructionErase"),
        new Resource.Logic.Operation(37, "position(OBJECT,NUM,NUM)", "InstructionPosition"),
        new Resource.Logic.Operation(38, "position.f(OBJECT,VAR,VAR)", "InstructionPositionV"),
        new Resource.Logic.Operation(39, "get.posn(OBJECT,VAR,VAR)", "InstructionGetPosition"),
        new Resource.Logic.Operation(40, "reposition(OBJECT,VAR,VAR)", "InstructionReposition"),
        new Resource.Logic.Operation(41, "set.view(OBJECT,VIEW)", "InstructionSetView"),
        new Resource.Logic.Operation(42, "set.view.f(OBJECT,VAR)", "InstructionSetViewV"),
        new Resource.Logic.Operation(43, "set.loop(OBJECT,NUM)", "InstructionSetLoop"),
        new Resource.Logic.Operation(44, "set.loop.f(OBJECT,VAR)", "InstructionSetLoopV"),
        new Resource.Logic.Operation(45, "fix.loop(OBJECT)", "InstructionFixLoop"),
        new Resource.Logic.Operation(46, "release.loop(OBJECT)", "InstructionReleaseLoop"),
        new Resource.Logic.Operation(47, "set.cel(OBJECT,NUM)", "InstructionSetCell"),
        new Resource.Logic.Operation(48, "set.cel.f(OBJECT,VAR)", "InstructionSetCellV"),
        new Resource.Logic.Operation(49, "last.cel(OBJECT,VAR)", "InstructionLastCell"),
        new Resource.Logic.Operation(50, "current.cel(OBJECT,VAR)", "InstructionCurrentCell"),
        new Resource.Logic.Operation(51, "current.loop(OBJECT,VAR)", "InstructionCurrentLoop"),
        new Resource.Logic.Operation(52, "current.view(OBJECT,VAR)", "InstructionCurrentView"),
        new Resource.Logic.Operation(53, "number.of.loops(OBJECT,VAR)", "InstructionLastLoop"),
        new Resource.Logic.Operation(54, "set.priority(OBJECT,NUM)", "InstructionSetPriority"),
        new Resource.Logic.Operation(55, "set.priority.f(OBJECT,VAR)", "InstructionSetPriorityV"),
        new Resource.Logic.Operation(56, "release.priority(OBJECT)", "InstructionReleasePriority"),
        new Resource.Logic.Operation(57, "get.priority(OBJECT,VAR)", "InstructionGetPriority"),
        new Resource.Logic.Operation(58, "stop.update(OBJECT)", "InstructionStopUpdate"),
        new Resource.Logic.Operation(59, "start.update(OBJECT)", "InstructionStartUpdate"),
        new Resource.Logic.Operation(60, "force.update(OBJECT)", "InstructionForceUpdate"),
        new Resource.Logic.Operation(61, "ignore.horizon(OBJECT)", "InstructionIgnoreHorizon"),
        new Resource.Logic.Operation(62, "observe.horizon(OBJECT)", "InstructionObserveHorizon"),
        new Resource.Logic.Operation(63, "set.horizon(NUM)", "InstructionSetHorizon"),
        new Resource.Logic.Operation(64, "object.on.water(OBJECT)", "InstructionObjectOnWater"),
        new Resource.Logic.Operation(65, "object.on.land(OBJECT)", "InstructionObjectOnLand"),
        new Resource.Logic.Operation(66, "object.on.anything(OBJECT)", "InstructionObjectOnAnything"),
        new Resource.Logic.Operation(67, "ignore.objs(OBJECT)", "InstructionIgnoreObjects"),
        new Resource.Logic.Operation(68, "observe.objs(OBJECT)", "InstructionObserveObjects"),
        new Resource.Logic.Operation(69, "distance(OBJECT,OBJECT,VAR)", "InstructionDistance"),
        new Resource.Logic.Operation(70, "stop.cycling(OBJECT)", "InstructionStopCycling"),
        new Resource.Logic.Operation(71, "start.cycling(OBJECT)", "InstructionStartCycling"),
        new Resource.Logic.Operation(72, "normal.cycle(OBJECT)", "InstructionNormalCycling"),
        new Resource.Logic.Operation(73, "end.of.loop(OBJECT,FLAG)", "InstructionEndOfLoop"),
        new Resource.Logic.Operation(74, "reverse.cycle(OBJECT)", "InstructionReverseCycling"),
        new Resource.Logic.Operation(75, "reverse.loop(OBJECT,FLAG)", "InstructionReverseLoop"),
        new Resource.Logic.Operation(76, "cycle.time(OBJECT,VAR)", "InstructionCycleTime"),
        new Resource.Logic.Operation(77, "stop.motion(OBJECT)", "InstructionStopMotion"),
        new Resource.Logic.Operation(78, "start.motion(OBJECT)", "InstructionStartMotion"),
        new Resource.Logic.Operation(79, "step.size(OBJECT,VAR)", "InstructionStepSize"),
        new Resource.Logic.Operation(80, "step.time(OBJECT,VAR)", "InstructionStepTime"),
        new Resource.Logic.Operation(81, "move.obj(OBJECT,NUM,NUM,NUM,FLAG)", "InstructionMoveObject"),
        new Resource.Logic.Operation(82, "move.obj.f(OBJECT,VAR,VAR,VAR,FLAG)", "InstructionMoveObjectV"),
        new Resource.Logic.Operation(83, "follow.ego(OBJECT,NUM,FLAG)", "InstructionFollowEgo"),
        new Resource.Logic.Operation(84, "wander(OBJECT)", "InstructionWander"),
        new Resource.Logic.Operation(85, "normal.motion(OBJECT)", "InstructionNormalMotion"),
        new Resource.Logic.Operation(86, "set.dir(OBJECT,VAR)", "InstructionSetDir"),
        new Resource.Logic.Operation(87, "get.dir(OBJECT,VAR)", "InstructionGetDir"),
        new Resource.Logic.Operation(88, "ignore.blocks(OBJECT)", "InstructionIgnoreBlocks"),
        new Resource.Logic.Operation(89, "observe.blocks(OBJECT)", "InstructionObserveBlocks"),
        new Resource.Logic.Operation(90, "block(NUM,NUM,NUM,NUM)", "InstructionBlock"),
        new Resource.Logic.Operation(91, "unblock()", "InstructionUnblock"),
        new Resource.Logic.Operation(92, "get(OBJECT)", "InstructionGet"),
        new Resource.Logic.Operation(93, "get.f(VAR)", "InstructionGetV"),
        new Resource.Logic.Operation(94, "drop(OBJECT)", "InstructionDrop"),
        new Resource.Logic.Operation(95, "put(OBJECT,VAR)", "InstructionPut"),
        new Resource.Logic.Operation(96, "put.f(VAR,VAR)", "InstructionPutV"),
        new Resource.Logic.Operation(97, "get.room.f(VAR,VAR)", "InstructionGetRoom"),
        new Resource.Logic.Operation(98, "load.sound(NUM)", "InstructionLoadSound"),
        new Resource.Logic.Operation(99, "sound(NUM,FLAG)", "InstructionPlaySound"),
        new Resource.Logic.Operation(100, "stop.sound()", "InstructionStopSound"),
        new Resource.Logic.Operation(101, "print(MSGNUM)", "InstructionPrint"),
        new Resource.Logic.Operation(102, "print.f(VAR)", "InstructionPrintV"),
        new Resource.Logic.Operation(103, "display(NUM,NUM,MSGNUM)", "InstructionDisplay"),
        new Resource.Logic.Operation(104, "display.f(VAR,VAR,VAR)", "InstructionDisplayV"),
        new Resource.Logic.Operation(105, "clear.lines(NUM,NUM,NUM)", "InstructionClearLine"),
        new Resource.Logic.Operation(106, "text.screen()", "InstructionTextScreen"),
        new Resource.Logic.Operation(107, "graphics()", "InstructionGraphics"),
        new Resource.Logic.Operation(108, "set.cursor.char(MSGNUM)", "InstructionSetCursorChar"),
        new Resource.Logic.Operation(109, "set.text.attribute(NUM,NUM)", "InstructionSetTextAttributes"),
        new Resource.Logic.Operation(110, "shake.screen(NUM)", "InstructionShakeScreen"),
        new Resource.Logic.Operation(111, "configure.screen(NUM,NUM,NUM)", "InstructionConfigureScreen"),
        new Resource.Logic.Operation(112, "status.line.on()", "InstructionStatusLineOn"),
        new Resource.Logic.Operation(113, "status.line.off()", "InstructionStatusLineOff"),
        new Resource.Logic.Operation(114, "set.string(NUM,MSGNUM)", "InstructionSetString"),
        new Resource.Logic.Operation(115, "get.string(NUM,MSGNUM,NUM,NUM,NUM)", "InstructionGetString"),
        new Resource.Logic.Operation(116, "word.to.string(NUM,NUM)", "InstructionWordToString"),
        new Resource.Logic.Operation(117, "parse(NUM)", "InstructionParse"),
        new Resource.Logic.Operation(118, "get.num(MSGNUM,VAR)", "InstructionGetNum"),
        new Resource.Logic.Operation(119, "prevent.input()", "InstructionPreventInput"),
        new Resource.Logic.Operation(120, "accept.input()", "InstructionAcceptInput"),
        new Resource.Logic.Operation(121, "set.key(NUM,NUM,NUM)", "InstructionSetKey"),
        new Resource.Logic.Operation(122, "add.to.pic(VIEW,NUM,NUM,NUM,NUM,NUM,NUM)", "InstructionAddToPic"),
        new Resource.Logic.Operation(123, "add.to.pic.f(VAR,VAR,VAR,VAR,VAR,VAR,VAR)", "InstructionAddToPicV"),
        new Resource.Logic.Operation(124, "status()", "InstructionStatus"),
        new Resource.Logic.Operation(125, "save.game()", "InstructionSaveGame"),
        new Resource.Logic.Operation(126, "restore.game()", "InstructionRestoreGame"),
        new Resource.Logic.Operation((int) sbyte.MaxValue, "init.disk()", "InstructionInitDisk"),
        new Resource.Logic.Operation(128, "restart.game()", "InstructionRestartGame"),
        new Resource.Logic.Operation(129, "show.obj(VIEW)", "InstructionShowObject"),
        new Resource.Logic.Operation(130, "random(NUM,NUM,VAR)", "InstructionRandom"),
        new Resource.Logic.Operation(131, "program.control()", "InstructionProgramControl"),
        new Resource.Logic.Operation(132, "player.control()", "InstructionPlayerControl"),
        new Resource.Logic.Operation(133, "obj.status.f(VAR)", "InstructionObjectStatus"),
        new Resource.Logic.Operation(134, "quit(NUM)", "InstructionQuit"),
        new Resource.Logic.Operation(135, "show.mem()", "InstructionShowMem"),
        new Resource.Logic.Operation(136, "pause()", "InstructionPause"),
        new Resource.Logic.Operation(137, "echo.line()", "InstructionEchoLine"),
        new Resource.Logic.Operation(138, "cancel.line()", "InstructionCancelLine"),
        new Resource.Logic.Operation(139, "init.joy()", "InstructionInitJoystick"),
        new Resource.Logic.Operation(140, "toggle.monitor()", "InstructionToggleMonitor"),
        new Resource.Logic.Operation(141, "version()", "InstructionVersion"),
        new Resource.Logic.Operation(142, "script.size(NUM)", "InstructionSetScriptSize"),
        new Resource.Logic.Operation(143, "set.game.id(MSGNUM)", "InstructionSetGameID"),
        new Resource.Logic.Operation(144, "log(MSGNUM)", "InstructionLog"),
        new Resource.Logic.Operation(145, "set.scan.start()", "InstructionSetScanStart"),
        new Resource.Logic.Operation(146, "reset.scan.start()", "InstructionSetScanStart"),
        new Resource.Logic.Operation(147, "reposition.to(OBJECT,NUM,NUM)", "InstructionPosition"),
        new Resource.Logic.Operation(148, "reposition.to.f(OBJECT,VAR,VAR)", "InstructionPositionV"),
        new Resource.Logic.Operation(149, "trace.on()", "InstructionTraceOn"),
        new Resource.Logic.Operation(150, "trace.info(NUM,NUM,NUM)", "InstructionTraceInfo"),
        new Resource.Logic.Operation(151, "print.at(MSGNUM,NUM,NUM,NUM)", "InstructionPrintAt"),
        new Resource.Logic.Operation(152, "print.at.v(VAR,NUM,NUM,NUM)", "InstructionPrintAtV"),
        new Resource.Logic.Operation(153, "discard.view.v(VAR)", "InstructionDiscardView"),
        new Resource.Logic.Operation(154, "clear.text.rect(NUM,NUM,NUM,NUM,NUM)", "InstructionClearTextRect"),
        new Resource.Logic.Operation(155, "set.upper.left(NUM,NUM)", "InstructionUpperLeft"),
        new Resource.Logic.Operation(156, "set.menu(MSGNUM)", "InstructionSetMenu"),
        new Resource.Logic.Operation(157, "set.menu.item(MSGNUM,NUM)", "InstructionSetMenuItem"),
        new Resource.Logic.Operation(158, "submit.menu()", "InstructionSubmitMenu"),
        new Resource.Logic.Operation(159, "enable.item(NUM)", "InstructionEnableItem"),
        new Resource.Logic.Operation(160, "disable.item(NUM)", "InstructionDisableItem"),
        new Resource.Logic.Operation(161, "menu.input()", "InstructionMenuInput"),
        new Resource.Logic.Operation(162, "show.obj.v(VAR)", "InstructionShowObject"),
        new Resource.Logic.Operation(163, "open.dialogue()", "InstructionOpenDialogue"),
        new Resource.Logic.Operation(164, "close.dialogue()", "InstructionCloseDialogue"),
        new Resource.Logic.Operation(165, "mul.n(VAR,NUM)", "InstructionMultiply"),
        new Resource.Logic.Operation(166, "mul.v(VAR,VAR)", "InstructionMultiplyV"),
        new Resource.Logic.Operation(167, "div.n(VAR,NUM)", "InstructionDivide"),
        new Resource.Logic.Operation(168, "div.v(VAR,VAR)", "InstructionDivideV"),
        new Resource.Logic.Operation(169, "close.window()", "InstructionCloseWindow"),
        new Resource.Logic.Operation(170, "set.simple(NUM)", "InstructionSetSimple"),
        new Resource.Logic.Operation(171, "push.script()", "InstructionPushScript"),
        new Resource.Logic.Operation(172, "pop.script()", "InstructionPopScript"),
        new Resource.Logic.Operation(173, "hold.key()", "InstructionHoldKey"),
        new Resource.Logic.Operation(174, "set.pri.base(NUM)", "InstructionSetPriorityBase"),
        new Resource.Logic.Operation(175, "discard.sound(NUM)", "InstructionDiscardSound"),
        new Resource.Logic.Operation(176, "hide.mouse()", "InstructionHideMouse"),
        new Resource.Logic.Operation(177, "allow.menu(NUM)", "InstructionAllowMenu"),
        new Resource.Logic.Operation(178, "show.mouse()", "InstructionShowMouse"),
        new Resource.Logic.Operation(179, "fence.mouse(NUM,NUM,NUM,NUM)", "InstructionFenceMouse"),
        new Resource.Logic.Operation(180, "mouse.posn(VAR,VAR)", "InstructionMousePosition"),
        new Resource.Logic.Operation(181, "release.key()", "InstructionReleaseKey"),
        new Resource.Logic.Operation(182, "adj.ego.move.to.x.y(NUM,NUM)", "InstructionAdjustEgoMoveToXY")
      };

      public List<Resource.Logic.Action> Actions { get; set; }

      public List<string> Messages { get; set; }

      public Dictionary<int, int> AddressToActionIndex { get; set; }

      public Logic(byte[] rawData, bool messagesCrypted = true)
      {
        this.Actions = new List<Resource.Logic.Action>();
        this.Messages = new List<string>();
        this.AddressToActionIndex = new Dictionary<int, int>();
        this.messagesCrypted = messagesCrypted;
        this.Decode(rawData);
      }

      public override byte[] Encode() => (byte[]) null;

      public void Decode(byte[] rawData)
      {
        this.ReadActions((Stream) new MemoryStream(rawData, 2, (int) rawData[0] + ((int) rawData[1] << 8)));
        this.ReadMessages(rawData);
      }

      private void ReadActions(Stream stream)
      {
        int num1 = 0;
        Resource.Logic.Action action1;
        while ((action1 = this.ReadAction(stream)) != null)
        {
          this.Actions.Add(action1);
          this.AddressToActionIndex.Add(action1.Address, num1++);
        }
        if (this.Actions[this.Actions.Count - 1].Operation.Opcode == 0)
          return;
        Resource.Logic.Action action2 = new Resource.Logic.Action(Resource.Logic.ACTION_OPERATIONS[0], new List<Resource.Logic.Operand>());
        action2.Address = this.Actions[this.Actions.Count - 1].Address + 1;
        action2.Logic = this;
        this.Actions.Add(action2);
        Dictionary<int, int> addressToActionIndex = this.AddressToActionIndex;
        int address = action2.Address;
        int num2 = num1;
        int num3 = num2 + 1;
        addressToActionIndex.Add(address, num2);
      }

      private Resource.Logic.Action ReadAction(Stream stream)
      {
        Resource.Logic.Action action = (Resource.Logic.Action) null;
        int position = (int) stream.Position;
        int index = stream.ReadByte();
        if (index >= 0)
        {
          if (index == (int) byte.MaxValue)
          {
            List<Resource.Logic.Operand> operands = new List<Resource.Logic.Operand>();
            List<Resource.Logic.Condition> conditionList = new List<Resource.Logic.Condition>();
            Resource.Logic.Condition condition;
            while ((condition = this.ReadCondition(stream, (int) byte.MaxValue)) != null)
              conditionList.Add(condition);
            operands.Add(new Resource.Logic.Operand(Resource.Logic.OperandType.TESTLIST, (object) conditionList));

                        int a = stream.ReadByte(), b = stream.ReadByte();
            operands.Add(new Resource.Logic.Operand(Resource.Logic.OperandType.ADDRESS, (object) ((int) (short) (a + (b << 8)) + (int) stream.Position), a, b));
            action = (Resource.Logic.Action) new Resource.Logic.IfAction(operands);
          }
          else if (index == 254)
          {
                        int a = stream.ReadByte(), b = stream.ReadByte();
            action = (Resource.Logic.Action) new Resource.Logic.GotoAction(new List<Resource.Logic.Operand>()
            {
              new Resource.Logic.Operand(Resource.Logic.OperandType.ADDRESS, (object) ((int) (short) (a + (b << 8)) + (int) stream.Position), a, b)
            });
          }
          else
          {
            Resource.Logic.Operation operation = Resource.Logic.ACTION_OPERATIONS[index];
            List<Resource.Logic.Operand> operands = new List<Resource.Logic.Operand>();
            foreach (Resource.Logic.OperandType operandType in operation.OperandTypes)
              operands.Add(new Resource.Logic.Operand(operandType, (object) stream.ReadByte()));
            action = new Resource.Logic.Action(operation, operands);
          }
          action.Address = position;
          action.Logic = this;
        }
        return action;
      }

      private Resource.Logic.Condition ReadCondition(Stream stream, int endCode)
      {
        Resource.Logic.Condition condition1 = (Resource.Logic.Condition) null;
        int num1 = (int) stream.Position - 2;
        int index1 = stream.ReadByte();
        if (index1 != endCode)
        {
          switch (index1)
          {
            case 14:
              Resource.Logic.Operation operation1 = Resource.Logic.TEST_OPERATIONS[index1];
              List<Resource.Logic.Operand> operands1 = new List<Resource.Logic.Operand>();
              List<int> intList = new List<int>();
              int num2 = stream.ReadByte();
              for (int index2 = 0; index2 < num2; ++index2)
                intList.Add(stream.ReadByte() + (stream.ReadByte() << 8));
              operands1.Add(new Resource.Logic.Operand(Resource.Logic.OperandType.WORDLIST, (object) intList));
              condition1 = new Resource.Logic.Condition(operation1, operands1);
              break;
            case 252:
              List<Resource.Logic.Operand> operands2 = new List<Resource.Logic.Operand>();
              List<Resource.Logic.Condition> conditionList = new List<Resource.Logic.Condition>();
              Resource.Logic.Condition condition2;
              while ((condition2 = this.ReadCondition(stream, 252)) != null)
                conditionList.Add(condition2);
              operands2.Add(new Resource.Logic.Operand(Resource.Logic.OperandType.TESTLIST, (object) conditionList));
              condition1 = (Resource.Logic.Condition) new Resource.Logic.OrCondition(operands2);
              break;
            case 253:
              condition1 = (Resource.Logic.Condition) new Resource.Logic.NotCondition(new List<Resource.Logic.Operand>()
              {
                new Resource.Logic.Operand(Resource.Logic.OperandType.TEST, (object) this.ReadCondition(stream, (int) byte.MaxValue))
              });
              break;
            default:
              Resource.Logic.Operation operation2 = Resource.Logic.TEST_OPERATIONS[index1];
              List<Resource.Logic.Operand> operands3 = new List<Resource.Logic.Operand>();
              foreach (Resource.Logic.OperandType operandType in operation2.OperandTypes)
                operands3.Add(new Resource.Logic.Operand(operandType, (object) stream.ReadByte()));
              condition1 = new Resource.Logic.Condition(operation2, operands3);
              break;
          }
          condition1.Address = num1;
          condition1.Logic = this;
        }
        return condition1;
      }

      private void ReadMessages(byte[] rawData)
      {
        int index1 = (int) rawData[0] + ((int) rawData[1] << 8) + 2;
        int num1 = (int) rawData[index1];
        int start = index1 + 3 + num1 * 2;
        if (this.messagesCrypted)
          this.Crypt(rawData, start, rawData.Length);
        this.Messages.Capacity = num1 + 1;
        this.Messages.Add("");
        int num2 = 1;
        int index2 = index1 + 3;
        while (num2 <= num1)
        {
          int num3 = (int) rawData[index2] + ((int) rawData[index2 + 1] << 8);
          string str = "";
          if (num3 > 0)
          {
            int index3;
            int num4 = index3 = num3 + (index1 + 1);
            do
              ;
            while (rawData[num4++] != (byte) 0);
            str = Encoding.GetEncoding(437).GetString(rawData, index3, num4 - index3 - 1);
          }
          this.Messages.Add(str);
          ++num2;
          index2 += 2;
        }
      }

      public static void AdjustCommandsForVersion(string version)
      {
        if (!version.Equals("2.089"))
          return;
        Resource.Logic.ACTION_OPERATIONS[134] = new Resource.Logic.Operation(134, "quit()", "InstructionQuit");
      }

      public abstract class Instruction
      {
        public Resource.Logic.Operation Operation { get; set; }

        public List<Resource.Logic.Operand> Operands { get; set; }

        public int Address { get; set; }

        public Resource.Logic Logic { get; set; }

        public Instruction(
          Resource.Logic.Operation operation,
          List<Resource.Logic.Operand> operands)
        {
          this.Operation = operation;
          this.Operands = operands;
        }
      }

      public class Condition : Resource.Logic.Instruction
      {
        public Condition(Resource.Logic.Operation operation, List<Resource.Logic.Operand> operands)
          : base(operation, operands)
        {
        }
      }

      public class Action : Resource.Logic.Instruction
      {
        public int ActionNumber => this.Logic.AddressToActionIndex[this.Address];

        public Action(Resource.Logic.Operation operation, List<Resource.Logic.Operand> operands)
          : base(operation, operands)
        {
        }
      }

      public abstract class JumpAction : Resource.Logic.Action
      {
        public JumpAction(Resource.Logic.Operation operation, List<Resource.Logic.Operand> operands)
          : base(operation, operands)
        {
        }

        public int GetDestinationActionIndex() => this.Logic.AddressToActionIndex[this.GetDestinationAddress()];

        public abstract int GetDestinationAddress();
      }

      public class IfAction : Resource.Logic.JumpAction
      {
        public IfAction(List<Resource.Logic.Operand> operands)
          : base(new Resource.Logic.Operation((int) byte.MaxValue, "if(TESTLIST,ADDRESS)", "InstructionIf"), operands)
        {
        }

        public override int GetDestinationAddress() => this.Operands[1].asInt();
      }

      public class GotoAction : Resource.Logic.JumpAction
      {
        public GotoAction(List<Resource.Logic.Operand> operands)
          : base(new Resource.Logic.Operation(254, "goto(ADDRESS)", "InstructionGoto"), operands)
        {
        }

        public override int GetDestinationAddress() => this.Operands[0].asInt();
      }

      public class NotCondition : Resource.Logic.Condition
      {
        public NotCondition(List<Resource.Logic.Operand> operands)
          : base(new Resource.Logic.Operation(253, "not(TEST)", "ExpressionNot"), operands)
        {
        }
      }

      public class OrCondition : Resource.Logic.Condition
      {
        public OrCondition(List<Resource.Logic.Operand> operands)
          : base(new Resource.Logic.Operation(252, "or(TESTLIST)", "ExpressionOr"), operands)
        {
        }
      }

      public class Operand
      {
        private object value;

        public Resource.Logic.OperandType OperandType { get; set; }

         public int A { get; set; }

         public int B { get; set; }

        public Operand(Resource.Logic.OperandType operandType, object value)
        {
          this.OperandType = operandType;
          this.value = value;
        }

        public Operand(Resource.Logic.OperandType operandType, object value, int a, int b): this(operandType, value)
        {
                    this.A = a;
                    this.B = b;
        }

        public int asInt() => (int) this.value;

        public short asShort() => (short) (int) this.value;

        public byte asByte() => (byte) (int) this.value;

        public sbyte asSByte() => (sbyte) (int) this.value;

        public Resource.Logic.Condition asCondition() => (Resource.Logic.Condition) this.value;

        public List<Resource.Logic.Condition> asConditions() => (List<Resource.Logic.Condition>) this.value;

        public List<int> asInts() => (List<int>) this.value;
      }

      public enum OperandType
      {
        VAR,
        NUM,
        FLAG,
        OBJECT,
        WORDLIST,
        VIEW,
        MSGNUM,
        TEST,
        TESTLIST,
        ADDRESS,
      }

      public class Operation
      {
        public int Opcode { get; }

        public string Format { get; }

        public string Name { get; }

        public List<Resource.Logic.OperandType> OperandTypes { get; set; }

        public string ExecutionClass { get; }

        public Operation(int opcode, string format, string executionClass)
        {
          this.Opcode = opcode;
          this.Format = format;
          this.ExecutionClass = executionClass;
          this.OperandTypes = new List<Resource.Logic.OperandType>();
          int length = format.IndexOf("(");
          int num = format.IndexOf(")");
          this.Name = format.Substring(0, length);
          if (num - length <= 1)
            return;
          string str1 = format.Substring(length + 1, num - length);
          char[] chArray = new char[1]{ ',' };
          foreach (string str2 in str1.Split(chArray))
          {
            Resource.Logic.OperandType result;
            Enum.TryParse<Resource.Logic.OperandType>(str2, out result);
            this.OperandTypes.Add(result);
          }
        }
      }
    }
  }
}
