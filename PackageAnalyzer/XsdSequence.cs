// Decompiled with JetBrains decompiler
// Type: PackageInspector.XsdSequence
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
  public class XsdSequence : ISearchProvider
  {
    public IList<XsdElement> Elements { get; private set; }

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
        return "Sequence";
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public XsdSequence(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode node = navigator as XmlNode;
      if (node == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Elements = (IList<XsdElement>) new List<XsdElement>();
      this.TraverseNode(node);
      this.PopulateSearchItems();
    }

    private void TraverseNode(XmlNode node)
    {
      if (node == null)
        return;
      foreach (XmlNode childNode in node.ChildNodes)
        this.TraverseNode(childNode);
      if (!(node.Name == "xsd:element"))
        return;
      this.Elements.Add(new XsdElement((IXPathNavigable) node));
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add("sequence");
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider element in (IEnumerable<XsdElement>) this.Elements)
        this.SubSearchProviders.Add(element);
    }
  }
}
