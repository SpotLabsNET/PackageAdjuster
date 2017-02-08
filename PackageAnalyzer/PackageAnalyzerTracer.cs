// Decompiled with JetBrains decompiler
// Type: PackageInspector.PackageAnalyzerTracer
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;

namespace PackageInspector
{
  public class PackageAnalyzerTracer
  {
    private static PackageAnalyzerTracer singleton;

    public static PackageAnalyzerTracer Instance
    {
      get
      {
        if (PackageAnalyzerTracer.singleton == null)
          PackageAnalyzerTracer.singleton = new PackageAnalyzerTracer();
        return PackageAnalyzerTracer.singleton;
      }
    }

    public event EventHandler<PackageAnalyzerInfoEventArgs> Info;

    private PackageAnalyzerTracer()
    {
    }

    public void Write(object message)
    {
      if (this.Info == null)
        return;
      this.Info((object) this, new PackageAnalyzerInfoEventArgs(message.ToString(), MessageScope.Internal));
    }

    public void Write(object message, MessageScope scope)
    {
      if (this.Info == null)
        return;
      this.Info((object) this, new PackageAnalyzerInfoEventArgs(message.ToString(), scope));
    }
  }
}
