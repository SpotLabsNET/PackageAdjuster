// Decompiled with JetBrains decompiler
// Type: PackageInspector.ParentAssemblyIdentityElement
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace PackageInspector
{
  public class ParentAssemblyIdentityElement : AssemblyIdentityElement, ISearchProvider
  {
    public string SearchIdentifier
    {
      get
      {
        return "Package Applicability (MUM)";
      }
    }

    public IList<string> Keywords { get; private set; }

    public string SearchPath
    {
      get
      {
        return "Parent Assembly Identity - " + string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}, {4}", (object) this.Name, (object) this.Version, (object) this.Architecture, (object) this.Language, (object) this.VersionScope);
      }
    }

    public IList<ISearchProvider> SubSearchProviders
    {
      get
      {
        return (IList<ISearchProvider>) null;
      }
    }

    public ParentAssemblyIdentityElement(IXPathNavigable navigator)
      : base(navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      if (!(navigator is XmlNode))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) this.AttributeMap)
      {
        this.Keywords.Add(attribute.Key);
        this.Keywords.Add(attribute.Value);
      }
    }
  }
}
