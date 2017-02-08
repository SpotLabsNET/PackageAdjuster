// Decompiled with JetBrains decompiler
// Type: PackageInspector.XsdSimpleType
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
  public class XsdSimpleType : ISearchProvider
  {
    public string Name { get; private set; }

    public XsdRestriction Restriction { get; private set; }

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
        return "Simple Type - " + this.Name;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public XsdSimpleType(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode node = navigator as XmlNode;
      if (node == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Name = node.Attributes["name"].Value;
      this.TraverseNode(node);
      this.PopulateSearchItems();
    }

    private void TraverseNode(XmlNode node)
    {
      if (node == null)
        return;
      foreach (XmlNode childNode in node.ChildNodes)
        this.TraverseNode(childNode);
      if (!(node.Name == "xsd:restriction"))
        return;
      this.Restriction = new XsdRestriction((IXPathNavigable) node);
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add(this.Name);
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      this.SubSearchProviders.Add((ISearchProvider) this.Restriction);
    }
  }
}
