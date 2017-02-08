// Decompiled with JetBrains decompiler
// Type: PackageInspector.RegistryKeyElement
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
  public class RegistryKeyElement : ISearchProvider
  {
    public string KeyName { get; private set; }

    public string Owner { get; private set; }

    public IList<RegistryValueElement> RegistryValues { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "Registry Key";
      }
    }

    public string SearchPath
    {
      get
      {
        return "Registry Key - " + this.KeyName;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public RegistryKeyElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.KeyName = string.Empty;
      this.Owner = string.Empty;
      if (xmlNode.Attributes["keyName"] != null)
        this.KeyName = xmlNode.Attributes["keyName"].Value;
      if (xmlNode.Attributes["owner"] != null)
        this.Owner = xmlNode.Attributes["owner"].Value;
      string uri = xmlNode.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      XmlNodeList xmlNodeList = xmlNode.SelectNodes("ns:registryValue", nsmgr);
      this.RegistryValues = (IList<RegistryValueElement>) new List<RegistryValueElement>();
      foreach (IXPathNavigable navigator1 in xmlNodeList)
        this.RegistryValues.Add(new RegistryValueElement(navigator1));
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add(this.KeyName);
      this.Keywords.Add(this.Owner);
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider registryValue in (IEnumerable<RegistryValueElement>) this.RegistryValues)
        this.SubSearchProviders.Add(registryValue);
    }
  }
}
