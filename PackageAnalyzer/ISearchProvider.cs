// Decompiled with JetBrains decompiler
// Type: PackageInspector.ISearchProvider
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System.Collections.Generic;

namespace PackageInspector
{
  public interface ISearchProvider
  {
    string SearchIdentifier { get; }

    IList<string> Keywords { get; }

    string SearchPath { get; }

    IList<ISearchProvider> SubSearchProviders { get; }
  }
}
