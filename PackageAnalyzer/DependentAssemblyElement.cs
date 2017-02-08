// Decompiled with JetBrains decompiler
// Type: PackageInspector.DependentAssemblyElement
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
  public class DependentAssemblyElement : ISearchProvider
  {
    public AssemblyIdentityElement AssemblyIdentity { get; private set; }

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
        return "Dependent Component - " + string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}, {4}", (object) this.AssemblyIdentity.Name, (object) this.AssemblyIdentity.Version, (object) this.AssemblyIdentity.Architecture, (object) this.AssemblyIdentity.Language, (object) this.AssemblyIdentity.VersionScope);
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders
    {
      get
      {
        return (IList<ISearchProvider>) null;
      }
    }

    public DependentAssemblyElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      string uri = xmlNode.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      this.AssemblyIdentity = new AssemblyIdentityElement((IXPathNavigable) xmlNode.SelectSingleNode("ns:assemblyIdentity", nsmgr));
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) this.AssemblyIdentity.AttributeMap)
      {
        this.Keywords.Add(attribute.Key);
        this.Keywords.Add(attribute.Value);
      }
    }
  }
}
