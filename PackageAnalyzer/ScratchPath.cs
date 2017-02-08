// Decompiled with JetBrains decompiler
// Type: PackageInspector.ScratchPath
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Globalization;

namespace PackageInspector
{
  public class ScratchPath
  {
    private static ScratchPath singleton;

    public string Folder { get; private set; }

    public static ScratchPath Instance
    {
      get
      {
        if (ScratchPath.singleton == null)
          ScratchPath.singleton = new ScratchPath();
        return ScratchPath.singleton;
      }
    }

    private ScratchPath()
    {
      this.Folder = Environment.GetEnvironmentVariable("TEMP") + "\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss", (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
