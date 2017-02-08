// Decompiled with JetBrains decompiler
// Type: PackageInspector.XsdComplexType
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
  public class XsdComplexType : IAttributeType, ISearchProvider
  {
    public string Name { get; private set; }

    public IDictionary<string, string> AttributeMap { get; private set; }

    public IList<XsdSequence> Sequences { get; private set; }

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
        return "Complex Type - " + this.Name;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public XsdComplexType(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode node = navigator as XmlNode;
      if (node == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Sequences = (IList<XsdSequence>) new List<XsdSequence>();
      this.Name = node.Attributes["name"].Value;
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
      this.TraverseNode(node);
      this.PopulateSearchItems();
    }

    private void TraverseNode(XmlNode node)
    {
      if (node == null)
        return;
      foreach (XmlNode childNode in node.ChildNodes)
        this.TraverseNode(childNode);
      if (!(node.Name == "xsd:sequence"))
        return;
      this.Sequences.Add(new XsdSequence((IXPathNavigable) node));
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add(this.Name);
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) this.AttributeMap)
      {
        this.Keywords.Add(attribute.Key);
        this.Keywords.Add(attribute.Value);
      }
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider sequence in (IEnumerable<XsdSequence>) this.Sequences)
        this.SubSearchProviders.Add(sequence);
    }
  }
}
