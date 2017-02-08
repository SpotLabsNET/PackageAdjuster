// Decompiled with JetBrains decompiler
// Type: PackageInspector.ParentElement
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
  public class ParentElement : IAttributeType, ISearchProvider
  {
    public IDictionary<string, string> AttributeMap { get; private set; }

    public IList<ParentAssemblyIdentityElement> AssemblyIdentities { get; private set; }

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
        return "Parent";
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public ParentElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.AssemblyIdentities = (IList<ParentAssemblyIdentityElement>) new List<ParentAssemblyIdentityElement>();
      string uri = xmlNode.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      foreach (IXPathNavigable selectNode in xmlNode.SelectNodes("ns:assemblyIdentity", nsmgr))
        this.AssemblyIdentities.Add(new ParentAssemblyIdentityElement(selectNode));
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
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
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider assemblyIdentity in (IEnumerable<ParentAssemblyIdentityElement>) this.AssemblyIdentities)
        this.SubSearchProviders.Add(assemblyIdentity);
    }
  }
}
