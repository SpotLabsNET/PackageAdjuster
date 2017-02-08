// Decompiled with JetBrains decompiler
// Type: PackageInspector.PackageAnalyzerException
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PackageInspector
{
  [Serializable]
  public class PackageAnalyzerException : Exception, ISerializable
  {
    public PackageAnalyzerException()
    {
    }

    public PackageAnalyzerException(string message)
      : base(message)
    {
    }

    public PackageAnalyzerException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected PackageAnalyzerException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
    }
  }
}
