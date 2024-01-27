// Decompiled with JetBrains decompiler
// Type: AGI.Game
// Assembly: AGILibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 276FBEE1-CB86-487F-BEF7-C8FB13C27F51
// Assembly location: C:\meka\Examples\meka\packages\AGILibrary\AGILibrary.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

namespace AGI
{
  public class Game : IDisposable
  {
    private bool disposed;
    public static int MaxNumberOfResources = 256;
    public static int MaxNumberOfVolFiles = 16;
    public static Game.Directory[] Directories = new Game.Directory[4]
    {
      Game.Directory.LogDir,
      Game.Directory.PicDir,
      Game.Directory.ViewDir,
      Game.Directory.SndDir
    };
    protected VolumeArray volumeArray = new VolumeArray();
    protected string gameFolder;
    public string v3GameSig;
    public string version;
    public bool IsModified;
    public LzwExpander lzwExpander;

    public VolumeArray Volumes => this.volumeArray;

    public Resource.Objects Objects { get; set; }

    public Resource.Words Words { get; set; }

    public string GameFolder => this.gameFolder;

    public Game(string folderName)
    {
      this.gameFolder = Game.ValidateFolder(folderName);
      this.lzwExpander = new LzwExpander();
      if (!this.DecodeGame())
        throw new Exception("Decode of game failed.");
      this.IsModified = false;
    }

    protected static string ValidateFolder(string folderName)
    {
      if (folderName.EndsWith("\\"))
        folderName.TrimEnd('\\');
      return System.IO.Directory.Exists(folderName) ? folderName : throw new ArgumentException("The folder '" + folderName + "' does not exist!", nameof (folderName));
    }

    public static Game Create(string folderName)
    {
      Game.ValidateFolder(folderName);
      ProgressDialog progressDialog = new ProgressDialog("Creating game");
      progressDialog.Begin();
      progressDialog.CurrentInfo = "Creating directories...";
      progressDialog.MaxProgress = Game.Directories.Length;
      try
      {
        foreach (Game.Directory directory in Game.Directories)
        {
          FileStream fileStream = File.Open(folderName + "\\" + directory.ToString(), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
          progressDialog.AddInfo(string.Format("*** Created '{0}'.", (object) fileStream.Name));
          progressDialog.Step();
          fileStream.Close();
        }
      }
      catch (Exception ex)
      {
        progressDialog.AddError("ERROR: Could not create a directory file!");
        progressDialog.AddInfo("Detail:\r\n" + ex.Message);
        progressDialog.EndFail();
        return (Game) null;
      }
      progressDialog.EndSuccess();
      return new Game(folderName);
    }

    private bool ReadObjectFile(ProgressDialog pDialog)
    {
      if (pDialog != null)
        pDialog.CurrentInfo = "Reading OBJECT file...";
      try
      {
        FileStream fileStream = File.Open(this.GameFolder + "\\OBJECT", FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        pDialog?.AddInfo(string.Format("*** Opened 'OBJECT', {0} bytes.", (object) fileStream.Length));
        byte[] numArray = new byte[fileStream.Length];
        if ((long) fileStream.Read(numArray, 0, numArray.Length) != fileStream.Length)
        {
          pDialog?.AddError("ERROR: Unexpected end of file while reading OBJECT file data!");
          fileStream.Close();
          return false;
        }
        fileStream.Close();
        this.Objects = new Resource.Objects(numArray);
        return true;
      }
      catch (FileNotFoundException ex)
      {
        if (pDialog != null)
        {
          pDialog.AddError("ERROR: The OBJECT file is missing!");
          pDialog.AddInfo("Detail:\r\n" + ex.Message);
          pDialog.AddInfo("");
          pDialog.AddInfo(string.Format("Are you sure the folder '{0}' contains an AGI v2 game?", (object) this.GameFolder));
          pDialog.EndFail();
        }
        return false;
      }
      catch (Exception ex)
      {
        if (pDialog != null)
        {
          pDialog.AddError("ERROR: Could not open OBJECT file!");
          pDialog.AddInfo("Detail:\r\n" + ex.Message);
          pDialog.EndFail();
        }
        return false;
      }
    }

    private bool ReadWordsFile(ProgressDialog pDialog)
    {
      if (pDialog != null)
        pDialog.CurrentInfo = "Reading WORDS.TOK file...";
      try
      {
        FileStream fileStream = File.Open(this.GameFolder + "\\WORDS.TOK", FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        pDialog?.AddInfo(string.Format("*** Opened 'WORDS.TOK', {0} bytes.", (object) fileStream.Length));
        byte[] numArray = new byte[fileStream.Length];
        if ((long) fileStream.Read(numArray, 0, numArray.Length) != fileStream.Length)
        {
          pDialog?.AddError("ERROR: Unexpected end of file while reading WORDS.TOK file data!");
          fileStream.Close();
          return false;
        }
        fileStream.Close();
        this.Words = new Resource.Words(numArray);
        return true;
      }
      catch (FileNotFoundException ex)
      {
        if (pDialog != null)
        {
          pDialog.AddError("ERROR: The WORDS.TOK file is missing!");
          pDialog.AddInfo("Detail:\r\n" + ex.Message);
          pDialog.AddInfo("");
          pDialog.AddInfo(string.Format("Are you sure the folder '{0}' contains an AGI v2 game?", (object) this.GameFolder));
          pDialog.EndFail();
        }
        return false;
      }
      catch (Exception ex)
      {
        if (pDialog != null)
        {
          pDialog.AddError("ERROR: Could not open WORDS.TOK file!");
          pDialog.AddInfo("Detail:\r\n" + ex.Message);
          pDialog.EndFail();
        }
        return false;
      }
    }

    private string ReadVersion(ProgressDialog pDialog)
    {
      string str = "Unknown";
      if (pDialog != null)
        pDialog.CurrentInfo = "Reading AGIDATA.OVL file...";
      try
      {
        FileStream fileStream = File.Open(this.GameFolder + "\\AGIDATA.OVL", FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        pDialog?.AddInfo(string.Format("*** Opened 'AGIDATA.OVL', {0} bytes.", (object) fileStream.Length));
        byte[] numArray = new byte[fileStream.Length];
        if ((long) fileStream.Read(numArray, 0, numArray.Length) != fileStream.Length)
        {
          pDialog?.AddError("ERROR: Unexpected end of file while reading AGIDATA.OVL file data!");
          fileStream.Close();
          return str;
        }
        fileStream.Close();
        for (int index1 = 0; index1 < numArray.Length; ++index1)
        {
          if (numArray[index1] == (byte) 86 && numArray[index1 + 1] == (byte) 101 && numArray[index1 + 2] == (byte) 114 && numArray[index1 + 3] == (byte) 115 && numArray[index1 + 4] == (byte) 105 && numArray[index1 + 5] == (byte) 111 && numArray[index1 + 6] == (byte) 110 && numArray[index1 + 7] == (byte) 32)
          {
            int index2 = index1 + 8;
            while (numArray[index2] != (byte) 0)
              ++index2;
            str = Encoding.ASCII.GetString(numArray, index1 + 8, index2 - (index1 + 8));
            break;
          }
        }
        return str;
      }
      catch (FileNotFoundException ex)
      {
        return str;
      }
      catch (Exception ex)
      {
        return str;
      }
    }

    private string ReadV3GameSig()
    {
      string[] files = System.IO.Directory.GetFiles(this.GameFolder, "*DIR");
      if (files.Length != 1)
        return (string) null;
      string[] strArray = files[0].Split('\\');
      string upper = strArray[strArray.Length - 1].ToUpper();
      return upper.Substring(0, upper.IndexOf("DIR"));
    }

    private bool ReadV2Resources(ProgressDialog pDialog)
    {
      if (pDialog != null)
        pDialog.CurrentInfo = "Reading directories...";
      FileStream[] fileStreamArray1 = new FileStream[Game.Directories.Length];
      FileStream[] fileStreamArray2 = new FileStream[Game.MaxNumberOfVolFiles];
      try
      {
        foreach (Game.Directory directory in Game.Directories)
        {
          FileStream fileStream = File.Open(this.GameFolder + "\\" + directory.ToString(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
          pDialog?.AddInfo(string.Format("*** Opened '{0}', {1} bytes.", (object) fileStream.Name, (object) fileStream.Length));
          if (fileStream.Length % 3L != 0L)
          {
            if (pDialog != null)
            {
              pDialog.AddError(string.Format("The file {0} is corrupt. ({1} % 3 == {2})", (object) fileStream.Name, (object) fileStream.Length, (object) (fileStream.Length % 3L)));
              pDialog.EndFail();
            }
            fileStream.Close();
            return false;
          }
          fileStreamArray1[(int) directory] = fileStream;
        }
      }
      catch (FileNotFoundException ex)
      {
        if (pDialog != null)
        {
          pDialog.AddError("ERROR: One or all of the directory files is missing!");
          pDialog.AddInfo("Detail:\r\n" + ex.Message);
          pDialog.AddInfo("");
          pDialog.AddInfo(string.Format("Are you sure the folder '{0}' contains an AGI v2 game?", (object) this.GameFolder));
          pDialog.EndFail();
        }
        return false;
      }
      catch (Exception ex)
      {
        if (pDialog != null)
        {
          pDialog.AddError("ERROR: Could not open a directory file!");
          pDialog.AddInfo("Detail:\r\n" + ex.Message);
          pDialog.EndFail();
        }
        return false;
      }
      if (pDialog != null)
        pDialog.MaxProgress = (int) (fileStreamArray1[0].Length / 3L + fileStreamArray1[1].Length / 3L + fileStreamArray1[2].Length / 3L + fileStreamArray1[3].Length / 3L);
      byte[] buffer1 = new byte[3];
      foreach (Game.Directory directory in Game.Directories)
      {
        if (pDialog != null)
          pDialog.CurrentInfo = "Processing " + directory.ToString() + "...";
        int num1 = 0;
        FileStream fileStream = fileStreamArray1[(int) directory];
        for (int index1 = 0; (long) index1 < fileStream.Length / 3L; ++index1)
        {
          if (pDialog != null && pDialog.Step())
          {
            pDialog.AddError("User abort!");
            pDialog.EndFail();
            return false;
          }
          if (pDialog != null)
            Thread.Sleep(1);
          try
          {
            if (fileStream.Read(buffer1, 0, 3) != 3)
            {
              if (pDialog != null)
              {
                pDialog.AddError("ERROR: Unexpected end of file while reading resource data position.");
                pDialog.EndFail();
              }
              return false;
            }
          }
          catch (Exception ex)
          {
            if (pDialog != null)
            {
              pDialog.AddError(string.Format("ERROR: Could not read from file {0}, position 0x{1:X}!", (object) fileStream.Name, (object) fileStream.Position));
              pDialog.AddInfo("Detail:\r\n" + ex.Message);
              pDialog.EndFail();
            }
            return false;
          }
          if (buffer1[0] != byte.MaxValue || buffer1[1] != byte.MaxValue || buffer1[2] != byte.MaxValue)
          {
            int index2 = ((int) buffer1[0] & 240) >> 4;
            int num2 = (((int) buffer1[0] & 15) << 16) + ((int) buffer1[1] << 8) + (int) buffer1[2];
            if (pDialog != null)
              pDialog.CurrentInfo = string.Format("Processing resource {3}\\{0} at vol.{1}:0x{2:X}:", (object) index1, (object) index2, (object) num2, (object) directory.ToString());
            if (fileStreamArray2[index2] == null)
            {
              try
              {
                fileStreamArray2[index2] = File.Open(this.GameFolder + "\\" + string.Format("vol.{0}", (object) index2), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
              }
              catch (FileNotFoundException ex)
              {
                if (pDialog != null)
                {
                  pDialog.AddError(string.Format("SKIPPING: A vol file is missing for resource {0}\\{1} (vol.{2})!", (object) directory.ToString(), (object) index1, (object) index2));
                  pDialog.AddInfo("Detail:\r\n" + ex.Message);
                  continue;
                }
                continue;
              }
              catch (Exception ex)
              {
                if (pDialog != null)
                {
                  pDialog.AddError(string.Format("SKIPPING: Resource {0}\\{1} in vol.{2}!", (object) directory.ToString(), (object) index1, (object) index2));
                  pDialog.AddInfo("Cause:\r\n" + ex.Message);
                  continue;
                }
                continue;
              }
              this.Volumes[index2] = new Volume();
              pDialog?.AddInfo(string.Format("*** Opened vol.{0}: 0x{1:X} bytes.", (object) index2, (object) fileStreamArray2[index2].Length));
            }
            if ((long) num2 > fileStreamArray2[index2].Length)
            {
              pDialog?.AddError(string.Format("SKIPPING: Resource header position 0x{0:X} is out of bounds. Skipping resource {1}\\{2}!", (object) num2, (object) directory.ToString(), (object) index1));
            }
            else
            {
              byte[] buffer2 = new byte[5];
              try
              {
                fileStreamArray2[index2].Position = (long) num2;
                if (fileStreamArray2[index2].Read(buffer2, 0, 5) != 5)
                {
                  if (pDialog != null)
                  {
                    pDialog.AddError("ERROR: Unexpected end of file while reading resource header.");
                    pDialog.EndFail();
                  }
                  return false;
                }
              }
              catch (Exception ex)
              {
                if (pDialog != null)
                {
                  pDialog.AddError(string.Format("ERROR: Could not read from file {0}!", (object) fileStreamArray2[index2].Name));
                  pDialog.AddInfo("Detail:\r\n" + ex.Message);
                  pDialog.EndFail();
                }
                return false;
              }
              if (buffer2[0] == (byte) 18 && buffer2[1] == (byte) 52)
              {
                if ((int) buffer2[2] != index2 && pDialog != null)
                  pDialog.AddError(string.Format("WARNING: Vol file number in header ({0}) does not match actual vol file ({1})!", (object) buffer2[2], (object) index2));
                int count = (int) buffer2[3] + ((int) buffer2[4] << 8);
                if ((long) count > fileStreamArray2[index2].Length)
                {
                  pDialog?.AddError(string.Format("SKIPPING: Resource length 0x{0:X} will cause out of bounds. Skipping resource {1}\\{2}!", (object) num2, (object) directory.ToString(), (object) index1));
                }
                else
                {
                  byte[] numArray = new byte[count];
                  try
                  {
                    if (fileStreamArray2[index2].Read(numArray, 0, count) != count)
                    {
                      if (pDialog != null)
                      {
                        pDialog.AddError(string.Format("SKIPPING: Unexpected end of file while reading resource data. Skipping resource {1}\\{2}!", (object) num2, (object) directory.ToString(), (object) index1));
                        continue;
                      }
                      continue;
                    }
                  }
                  catch (Exception ex)
                  {
                    if (pDialog != null)
                    {
                      pDialog.AddError(string.Format("ERROR: Could not read from file {0}!", (object) fileStreamArray2[index2].Name));
                      pDialog.AddInfo("Detail:\r\n" + ex.Message);
                      pDialog.EndFail();
                    }
                    return false;
                  }
                  try
                  {
                    switch (directory)
                    {
                      case Game.Directory.LogDir:
                        this.Volumes[index2].Logics[index1] = (Resource) new Resource.Logic(numArray);
                        break;
                      case Game.Directory.PicDir:
                        this.Volumes[index2].Pictures[index1] = (Resource) new Resource.Picture(numArray);
                        break;
                      case Game.Directory.ViewDir:
                        this.Volumes[index2].Views[index1] = (Resource) new Resource.View(numArray);
                        break;
                      case Game.Directory.SndDir:
                        this.Volumes[index2].Sounds[index1] = (Resource) new Resource.Sound(numArray);
                        break;
                    }
                    ++num1;
                  }
                  catch (Exception ex)
                  {
                    if (pDialog != null)
                    {
                      pDialog.AddError("SKIPPING: Could not decode resource, index = " + index1.ToString() + ". The error given was:");
                      pDialog.AddInfo("\t" + ex.Message);
                      if (ex.InnerException != null)
                        pDialog.AddInfo("\t" + ex.InnerException.Message);
                    }
                  }
                }
              }
              else
                pDialog?.AddError(string.Format("SKIPPING: Skipping resource {0}, vol file is corrupt! (expected 0x1234 as header ID, found 0x{1:X})", (object) index1, (object) (((int) buffer2[0] << 8) + (int) buffer2[1])));
            }
          }
        }
        pDialog?.AddInfo(string.Format("Successfully read {0} resources from '{1}'.", (object) num1, (object) directory.ToString()));
      }
      foreach (FileStream fileStream in fileStreamArray1)
        fileStream?.Close();
      foreach (FileStream fileStream in fileStreamArray2)
        fileStream?.Close();
      return true;
    }

    private bool ReadV3Resources(ProgressDialog pDialog)
    {
      if (pDialog != null)
        pDialog.CurrentInfo = "Reading directories...";
      FileStream[] fileStreamArray = new FileStream[Game.MaxNumberOfVolFiles];
      FileStream fileStream1 = File.Open(this.GameFolder + "\\" + (this.v3GameSig.ToUpper() + "DIR"), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
      pDialog?.AddInfo(string.Format("*** Opened '{0}', {1} bytes.", (object) fileStream1.Name, (object) fileStream1.Length));
      if (pDialog != null)
        pDialog.MaxProgress = (int) (fileStream1.Length / 3L);
      byte[] buffer1 = new byte[3];
      foreach (Game.Directory directory in Game.Directories)
      {
        if (pDialog != null)
          pDialog.CurrentInfo = "Processing " + directory.ToString() + "...";
        int num1 = 0;
        int num2 = (int) directory;
        fileStream1.Position = (long) (num2 * 2);
        int num3 = fileStream1.ReadByte();
        int num4 = (fileStream1.ReadByte() << 8) + num3;
        int num5;
        if (num2 < 3)
        {
          fileStream1.Position = (long) ((num2 + 1) * 2);
          int num6 = fileStream1.ReadByte();
          num5 = (fileStream1.ReadByte() << 8) + num6;
        }
        else
          num5 = (int) fileStream1.Length;
        int num7 = num5 - num4;
        for (int index1 = 0; index1 < num7 / 3; ++index1)
        {
          fileStream1.Position = (long) (num4 + index1 * 3);
          if (pDialog != null && pDialog.Step())
          {
            pDialog.AddError("User abort!");
            pDialog.EndFail();
            return false;
          }
          if (pDialog != null)
            Thread.Sleep(1);
          try
          {
            if (fileStream1.Read(buffer1, 0, 3) != 3)
            {
              if (pDialog != null)
              {
                pDialog.AddError("ERROR: Unexpected end of file while reading resource data position.");
                pDialog.EndFail();
              }
              return false;
            }
          }
          catch (Exception ex)
          {
            if (pDialog != null)
            {
              pDialog.AddError(string.Format("ERROR: Could not read from file {0}, position 0x{1:X}!", (object) fileStream1.Name, (object) fileStream1.Position));
              pDialog.AddInfo("Detail:\r\n" + ex.Message);
              pDialog.EndFail();
            }
            return false;
          }
          if (buffer1[0] != byte.MaxValue || buffer1[1] != byte.MaxValue || buffer1[2] != byte.MaxValue)
          {
            int index2 = ((int) buffer1[0] & 240) >> 4;
            int num8 = (((int) buffer1[0] & 15) << 16) + ((int) buffer1[1] << 8) + (int) buffer1[2];
            if (pDialog != null)
              pDialog.CurrentInfo = string.Format("Processing resource {3}\\{0} at {4}vol.{1}:0x{2:X}:", (object) index1, (object) index2, (object) num8, (object) directory.ToString(), (object) this.v3GameSig.ToLower());
            if (fileStreamArray[index2] == null)
            {
              try
              {
                fileStreamArray[index2] = File.Open(this.GameFolder + "\\" + string.Format("{1}vol.{0}", (object) index2, (object) this.v3GameSig.ToLower()), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
              }
              catch (FileNotFoundException ex)
              {
                if (pDialog != null)
                {
                  pDialog.AddError(string.Format("SKIPPING: A vol file is missing for resource {0}\\{1} ({3}vol.{2})!", (object) directory.ToString(), (object) index1, (object) index2, (object) this.v3GameSig.ToLower()));
                  pDialog.AddInfo("Detail:\r\n" + ex.Message);
                  continue;
                }
                continue;
              }
              catch (Exception ex)
              {
                if (pDialog != null)
                {
                  pDialog.AddError(string.Format("SKIPPING: Resource {0}\\{1} in {3}vol.{2}!", (object) directory.ToString(), (object) index1, (object) index2, (object) this.v3GameSig.ToLower()));
                  pDialog.AddInfo("Cause:\r\n" + ex.Message);
                  continue;
                }
                continue;
              }
              this.Volumes[index2] = new Volume();
              pDialog?.AddInfo(string.Format("*** Opened {2}vol.{0}: 0x{1:X} bytes.", (object) index2, (object) fileStreamArray[index2].Length, (object) this.v3GameSig.ToLower()));
            }
            if ((long) num8 > fileStreamArray[index2].Length)
            {
              pDialog?.AddError(string.Format("SKIPPING: Resource header position 0x{0:X} is out of bounds. Skipping resource {1}\\{2}!", (object) num8, (object) directory.ToString(), (object) index1));
            }
            else
            {
              byte[] buffer2 = new byte[7];
              try
              {
                fileStreamArray[index2].Position = (long) num8;
                if (fileStreamArray[index2].Read(buffer2, 0, 7) != 7)
                {
                  if (pDialog != null)
                  {
                    pDialog.AddError("ERROR: Unexpected end of file while reading resource header.");
                    pDialog.EndFail();
                  }
                  return false;
                }
              }
              catch (Exception ex)
              {
                if (pDialog != null)
                {
                  pDialog.AddError(string.Format("ERROR: Could not read from file {0}!", (object) fileStreamArray[index2].Name));
                  pDialog.AddInfo("Detail:\r\n" + ex.Message);
                  pDialog.EndFail();
                }
                return false;
              }
              if (buffer2[0] == (byte) 18 && buffer2[1] == (byte) 52)
              {
                int num9 = (int) buffer2[2];
                int length = ((int) buffer2[4] << 8) + (int) buffer2[3];
                int count = ((int) buffer2[6] << 8) + (int) buffer2[5];
                if ((long) count > fileStreamArray[index2].Length)
                {
                  pDialog?.AddError(string.Format("SKIPPING: Resource length 0x{0:X} will cause out of bounds. Skipping resource {1}\\{2}!", (object) num8, (object) directory.ToString(), (object) index1));
                }
                else
                {
                  byte[] numArray1 = new byte[count];
                  try
                  {
                    if (fileStreamArray[index2].Read(numArray1, 0, count) != count)
                    {
                      if (pDialog != null)
                      {
                        pDialog.AddError(string.Format("SKIPPING: Unexpected end of file while reading resource data. Skipping resource {1}\\{2}!", (object) num8, (object) directory.ToString(), (object) index1));
                        continue;
                      }
                      continue;
                    }
                  }
                  catch (Exception ex)
                  {
                    if (pDialog != null)
                    {
                      pDialog.AddError(string.Format("ERROR: Could not read from file {0}!", (object) fileStreamArray[index2].Name));
                      pDialog.AddInfo("Detail:\r\n" + ex.Message);
                      pDialog.EndFail();
                    }
                    return false;
                  }
                  byte[] numArray2 = new byte[length];
                  bool messagesCrypted = false;
                  if ((num9 & 128) == 128)
                  {
                    if (!this.DecompressV3Picture(numArray1, numArray2))
                      return false;
                  }
                  else if (count == length)
                  {
                    numArray2 = numArray1;
                    if (directory == Game.Directory.LogDir)
                      messagesCrypted = true;
                  }
                  else if (!this.DecompressLZWResource(numArray1, numArray2))
                    return false;
                  try
                  {
                    switch (directory)
                    {
                      case Game.Directory.LogDir:
                        this.Volumes[index2].Logics[index1] = (Resource) new Resource.Logic(numArray2, messagesCrypted);
                        break;
                      case Game.Directory.PicDir:
                        this.Volumes[index2].Pictures[index1] = (Resource) new Resource.Picture(numArray2);
                        break;
                      case Game.Directory.ViewDir:
                        this.Volumes[index2].Views[index1] = (Resource) new Resource.View(numArray2);
                        break;
                      case Game.Directory.SndDir:
                        this.Volumes[index2].Sounds[index1] = (Resource) new Resource.Sound(numArray2);
                        break;
                    }
                    ++num1;
                  }
                  catch (Exception ex)
                  {
                    if (pDialog != null)
                    {
                      pDialog.AddError("SKIPPING: Could not decode resource, index = " + index1.ToString() + ". The error given was:");
                      pDialog.AddInfo("\t" + ex.Message);
                      if (ex.InnerException != null)
                        pDialog.AddInfo("\t" + ex.InnerException.Message);
                    }
                  }
                }
              }
              else
                pDialog?.AddError(string.Format("SKIPPING: Skipping resource {0}, vol file is corrupt! (expected 0x1234 as header ID, found 0x{1:X})", (object) index1, (object) (((int) buffer2[0] << 8) + (int) buffer2[1])));
            }
          }
        }
        pDialog?.AddInfo(string.Format("Successfully read {0} resources from '{1}'.", (object) num1, (object) directory.ToString()));
      }
      fileStream1.Close();
      foreach (FileStream fileStream2 in fileStreamArray)
        fileStream2?.Close();
      return true;
    }

    private bool DecompressLZWResource(byte[] compressedBuffer, byte[] uncompressedBuffer) => this.lzwExpander.Expand(compressedBuffer, uncompressedBuffer);

    private bool DecompressV3Picture(byte[] compressedBuffer, byte[] uncompressedBuffer)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      bool flag = false;
      while (num2 < compressedBuffer.Length)
      {
        int num4 = (int) compressedBuffer[num2++];
        int num5 = flag ? ((num4 & 240) >> 4) + ((num1 & 15) << 4) : num4;
        switch (num5)
        {
          case 240:
          case 242:
            byte[] numArray1 = uncompressedBuffer;
            int index1 = num3;
            int num6 = index1 + 1;
            int num7 = (int) (byte) num5;
            numArray1[index1] = (byte) num7;
            if (flag)
            {
              byte[] numArray2 = uncompressedBuffer;
              int index2 = num6;
              num3 = index2 + 1;
              int num8 = (int) (byte) (num4 & 15);
              numArray2[index2] = (byte) num8;
            }
            else
            {
              num4 = (int) compressedBuffer[num2++];
              byte[] numArray3 = uncompressedBuffer;
              int index3 = num6;
              num3 = index3 + 1;
              int num9 = (int) (byte) ((num4 & 240) >> 4);
              numArray3[index3] = (byte) num9;
            }
            flag = !flag;
            break;
          default:
            uncompressedBuffer[num3++] = (byte) num5;
            break;
        }
        num1 = num4;
      }
      return true;
    }

    private bool DecodeGame()
    {
      ProgressDialog pDialog = (ProgressDialog) null;
      pDialog?.Begin();
      if (!this.ReadObjectFile(pDialog) || !this.ReadWordsFile(pDialog))
        return false;
      this.v3GameSig = this.ReadV3GameSig();
      this.version = this.ReadVersion(pDialog);
      Resource.Logic.AdjustCommandsForVersion(this.version);
      if (this.v3GameSig != null)
      {
        if (!this.ReadV3Resources(pDialog))
          return false;
      }
      else if (!this.ReadV2Resources(pDialog))
        return false;
      pDialog?.EndSuccess();
      return true;
    }

    public bool Save()
    {
      ProgressDialog progressDialog = new ProgressDialog("Encoding game");
      progressDialog.Begin();
      progressDialog.CurrentInfo = "Creating directories...";
      progressDialog.MaxProgress = this.Volumes.Count;
      FileStream[] fileStreamArray1 = new FileStream[Game.Directories.Length];
      FileStream[] fileStreamArray2 = new FileStream[Game.MaxNumberOfVolFiles];
      try
      {
        foreach (Game.Directory directory in Game.Directories)
        {
          FileStream fileStream = File.Open(this.GameFolder + "\\" + directory.ToString(), FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
          progressDialog.AddInfo(string.Format("*** Created '{0}'.", (object) fileStream.Name));
          progressDialog.Step();
          fileStreamArray1[(int) directory] = fileStream;
        }
      }
      catch (Exception ex)
      {
        progressDialog.AddError("ERROR: Could not create a directory file!");
        progressDialog.AddInfo("Detail:\r\n" + ex.Message);
        progressDialog.EndFail();
        return false;
      }
      foreach (Volume volume in (CollectionBase) this.Volumes)
      {
        if (fileStreamArray2[volume.Index] == null)
        {
          try
          {
            fileStreamArray2[volume.Index] = File.Open(this.GameFolder + "\\vol." + volume.Index.ToString(), FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
          }
          catch (Exception ex)
          {
            progressDialog.AddError("ERROR: Could not create a volume file!");
            progressDialog.AddInfo("Detail:\r\n" + ex.Message);
            progressDialog.EndFail();
            return false;
          }
          progressDialog.AddInfo(string.Format("*** Created {0}", (object) fileStreamArray2[volume.Index].Name));
        }
        foreach (Game.Directory directory in Game.Directories)
        {
          ResourceArray resourceArray = volume[directory];
          FileStream fileStream = fileStreamArray1[(int) directory];
          if ((long) (resourceArray.LargestIndex * 3) > fileStream.Length)
          {
            fileStream.Position = fileStream.Length;
            while (fileStream.Position < (long) (resourceArray.LargestIndex * 3))
              fileStream.WriteByte(byte.MaxValue);
          }
          foreach (Resource resource in (CollectionBase) resourceArray)
          {
            progressDialog.CurrentInfo = "Writing " + this.GetPath(resource);
            byte[] buffer1;
            try
            {
              buffer1 = resource.Encode();
            }
            catch (Exception ex)
            {
              progressDialog.AddError(string.Format("SKIPPING: Could not encode resource {0}!", (object) this.GetPath(resource)));
              progressDialog.AddInfo("Detail:\r\n" + ex.Message);
              continue;
            }
            if (buffer1 == null)
            {
              progressDialog.AddError(string.Format("SKIPPING: Encoding resource {0} returned no data!", (object) this.GetPath(resource)));
            }
            else
            {
              byte[] buffer2 = new byte[3];
              long position = fileStreamArray2[volume.Index].Position;
              buffer2[0] = (byte) ((volume.Index & 15) << 4);
              buffer2[0] += (byte) ((position & 983040L) >> 16);
              buffer2[1] = (byte) ((position & 65280L) >> 8);
              buffer2[2] = (byte) ((ulong) position & (ulong) byte.MaxValue);
              byte[] buffer3 = new byte[5]
              {
                (byte) 18,
                (byte) 52,
                (byte) (volume.Index & 15),
                (byte) (buffer1.Length & (int) byte.MaxValue),
                (byte) ((buffer1.Length & 65280) >> 8)
              };
              try
              {
                fileStreamArray2[volume.Index].Write(buffer3, 0, 5);
                fileStreamArray2[volume.Index].Write(buffer1, 0, buffer1.Length);
              }
              catch (Exception ex)
              {
                progressDialog.AddError(string.Format("ERROR: Could not write to file {0}!", (object) fileStreamArray2[volume.Index].Name));
                progressDialog.AddInfo("Detail:\r\n" + ex.Message);
                progressDialog.EndFail();
                return false;
              }
              try
              {
                fileStream.Position = (long) (resource.Index * 3);
                fileStream.Write(buffer2, 0, 3);
              }
              catch (Exception ex)
              {
                progressDialog.AddError(string.Format("ERROR: Could not write to file {0}, position 0x{1:X}!", (object) fileStream.Name, (object) fileStream.Position));
                progressDialog.AddInfo("Detail:\r\n" + ex.Message);
                progressDialog.EndFail();
                return false;
              }
            }
          }
        }
        progressDialog.Step();
      }
      foreach (FileStream fileStream in fileStreamArray1)
        fileStream?.Close();
      foreach (FileStream fileStream in fileStreamArray2)
        fileStream?.Close();
      progressDialog.EndSuccess();
      return true;
    }

    public void Close() => this.Dispose();

    public string GetPath(Resource resource)
    {
      foreach (Volume volume in (CollectionBase) this.Volumes)
      {
        int num1;
        if ((num1 = volume.Pictures.IndexOf(resource)) >= 0)
          return string.Format("{2}\\vol.{0}\\picture.{1}", (object) this.Volumes.IndexOf(volume), (object) num1, (object) this.GameFolder);
        int num2;
        if ((num2 = volume.Views.IndexOf(resource)) >= 0)
          return string.Format("{2}\\vol.{0}\\view.{1}", (object) this.Volumes.IndexOf(volume), (object) num2, (object) this.GameFolder);
        int num3;
        if ((num3 = volume.Sounds.IndexOf(resource)) >= 0)
          return string.Format("{2}\\vol.{0}\\sound.{1}", (object) this.Volumes.IndexOf(volume), (object) num3, (object) this.GameFolder);
        int num4;
        if ((num4 = volume.Logics.IndexOf(resource)) >= 0)
          return string.Format("{2}\\vol.{0}\\logic.{1}", (object) this.Volumes.IndexOf(volume), (object) num4, (object) this.GameFolder);
      }
      return "Unknown resource";
    }

    public void Dispose()
    {
      if (this.disposed)
        return;
      foreach (Volume volume in (CollectionBase) this.Volumes)
        volume.Dispose();
      this.volumeArray.Clear();
      this.volumeArray = (VolumeArray) null;
    }

    public enum Directory
    {
      LogDir,
      PicDir,
      ViewDir,
      SndDir,
    }
  }
}
