// Decompiled with JetBrains decompiler
// Type: PackageInspector.PackageAnalyzerInfoEventArgs
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;

namespace PackageInspector
{
  public class PackageAnalyzerInfoEventArgs : EventArgs
  {
    public string Message { get; set; }

    public MessageScope Scope { get; set; }

    public PackageAnalyzerInfoEventArgs(string message, MessageScope scope)
    {
      if (message == null)
        throw new ArgumentNullException("message");
      this.Message = message;
      this.Scope = scope;
    }
  }
}
