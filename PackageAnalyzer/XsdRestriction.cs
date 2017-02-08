// Decompiled with JetBrains decompiler
// Type: PackageInspector.XsdRestriction
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
  public class XsdRestriction : ISearchProvider
  {
    public string Base { get; private set; }

    public IList<XsdEnumeration> Enumerations { get; private set; }

    public XsdMinInclusive MinInclusive { get; private set; }

    public XsdMaxInclusive MaxInclusive { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "SMI Setting";
      }
    }

    public string SearchPath
    {
      get
      {
        return "Restriction - " + this.Base;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public XsdRestriction(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode node = navigator as XmlNode;
      if (node == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Base = node.Attributes["base"].Value;
      this.Enumerations = (IList<XsdEnumeration>) new List<XsdEnumeration>();
      this.TraverseNode(node);
      this.PopulateSearchItems();
    }

    private void TraverseNode(XmlNode node)
    {
      if (node == null)
        return;
      foreach (XmlNode childNode in node.ChildNodes)
        this.TraverseNode(childNode);
      if (node.Name == "xsd:enumeration")
        this.Enumerations.Add(new XsdEnumeration((IXPathNavigable) node));
      if (node.Name == "xsd:minInclusive")
        this.MinInclusive = new XsdMinInclusive((IXPathNavigable) node);
      if (!(node.Name == "xsd:maxInclusive"))
        return;
      this.MaxInclusive = new XsdMaxInclusive((IXPathNavigable) node);
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add("restriction");
      this.Keywords.Add(this.Base);
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      this.SubSearchProviders.Add((ISearchProvider) this.MinInclusive);
      this.SubSearchProviders.Add((ISearchProvider) this.MaxInclusive);
      foreach (ISearchProvider enumeration in (IEnumerable<XsdEnumeration>) this.Enumerations)
        this.SubSearchProviders.Add(enumeration);
    }
  }
}
