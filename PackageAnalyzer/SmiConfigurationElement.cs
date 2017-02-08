// Decompiled with JetBrains decompiler
// Type: PackageInspector.SmiConfigurationElement
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace PackageInspector
{
  [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
  public class SmiConfigurationElement : ISearchProvider
  {
    public IList<XsdElement> Elements { get; private set; }

    public IList<XsdComplexType> ComplexTypes { get; private set; }

    public IList<XsdSimpleType> SimpleTypes { get; private set; }

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
        return "SMI Setting";
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public SmiConfigurationElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode node = navigator as XmlNode;
      if (node == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Elements = (IList<XsdElement>) new List<XsdElement>();
      this.ComplexTypes = (IList<XsdComplexType>) new List<XsdComplexType>();
      this.SimpleTypes = (IList<XsdSimpleType>) new List<XsdSimpleType>();
      this.TraverseNode(node);
      this.PopulateSearchItems();
    }

    private void TraverseNode(XmlNode node)
    {
      if (node == null)
        return;
      foreach (XmlNode childNode in node.ChildNodes)
        this.TraverseNode(childNode);
      if (node.Name == "xsd:element")
      {
        if (node.ParentNode.Name != "xsd:schema")
          return;
        this.Elements.Add(new XsdElement((IXPathNavigable) node));
      }
      else if (node.Name == "xsd:complexType")
      {
        this.ComplexTypes.Add(new XsdComplexType((IXPathNavigable) node));
      }
      else
      {
        if (!(node.Name == "xsd:simpleType"))
          return;
        this.SimpleTypes.Add(new XsdSimpleType((IXPathNavigable) node));
      }
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider element in (IEnumerable<XsdElement>) this.Elements)
        this.SubSearchProviders.Add(element);
      foreach (ISearchProvider simpleType in (IEnumerable<XsdSimpleType>) this.SimpleTypes)
        this.SubSearchProviders.Add(simpleType);
      foreach (ISearchProvider complexType in (IEnumerable<XsdComplexType>) this.ComplexTypes)
        this.SubSearchProviders.Add(complexType);
    }
  }
}
