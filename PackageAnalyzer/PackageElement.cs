// Decompiled with JetBrains decompiler
// Type: PackageInspector.PackageElement
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
  public class PackageElement : IAttributeType, ISearchProvider
  {
    public IDictionary<string, string> AttributeMap { get; private set; }

    public IList<ParentElement> Parents { get; private set; }

    public IList<UpdateElement> Updates { get; private set; }

    public string Identifier { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "Package Applicability (MUM)";
      }
    }

    public string SearchPath
    {
      get
      {
        return this.Identifier;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public PackageElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Parents = (IList<ParentElement>) new List<ParentElement>();
      this.Updates = (IList<UpdateElement>) new List<UpdateElement>();
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      string uri = xmlNode.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      foreach (IXPathNavigable selectNode in xmlNode.SelectNodes("ns:parent", nsmgr))
        this.Parents.Add(new ParentElement(selectNode));
      foreach (IXPathNavigable selectNode in xmlNode.SelectNodes("ns:update", nsmgr))
        this.Updates.Add(new UpdateElement(selectNode));
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
      this.Identifier = !this.AttributeMap.ContainsKey("identifier") ? string.Empty : this.AttributeMap["identifier"];
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add(this.Identifier);
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) this.AttributeMap)
      {
        this.Keywords.Add(attribute.Key);
        this.Keywords.Add(attribute.Value);
      }
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider parent in (IEnumerable<ParentElement>) this.Parents)
        this.SubSearchProviders.Add(parent);
      foreach (ISearchProvider update in (IEnumerable<UpdateElement>) this.Updates)
        this.SubSearchProviders.Add(update);
    }
  }
}
