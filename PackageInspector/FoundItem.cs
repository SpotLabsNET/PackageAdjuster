// Decompiled with JetBrains decompiler
// Type: PackageInspector.FoundItem
// Assembly: PackageInspector, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E560708D-177B-49EF-9D68-2DECA14F8A1B
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageInspector.exe

using System.Collections.Generic;

namespace PackageInspector
{
  public class FoundItem
  {
    public string Keyword { get; set; }

    public string Path { get; set; }

    public IList<ISearchProvider> SearchProviders { get; set; }

    public FoundItem()
    {
      this.SearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
    }
  }
}
