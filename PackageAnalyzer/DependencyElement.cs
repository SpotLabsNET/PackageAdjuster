// Decompiled with JetBrains decompiler
// Type: PackageInspector.DependencyElement
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
  public class DependencyElement : ISearchProvider
  {
    public string Discoverable { get; private set; }

    public IList<DependentAssemblyElement> DependentAssemblies { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "Dependent Component";
      }
    }

    public string SearchPath
    {
      get
      {
        return "Dependency";
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public DependencyElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Discoverable = string.Empty;
      if (xmlNode.Attributes["discoverable"] != null)
        this.Discoverable = xmlNode.Attributes["discoverable"].Value;
      XmlNode firstChild = xmlNode.FirstChild;
      this.DependentAssemblies = (IList<DependentAssemblyElement>) new List<DependentAssemblyElement>();
      this.DependentAssemblies.Add(new DependentAssemblyElement((IXPathNavigable) firstChild));
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider dependentAssembly in (IEnumerable<DependentAssemblyElement>) this.DependentAssemblies)
        this.SubSearchProviders.Add(dependentAssembly);
    }
  }
}
