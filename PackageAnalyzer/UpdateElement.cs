// Decompiled with JetBrains decompiler
// Type: PackageInspector.UpdateElement
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
  public class UpdateElement : IAttributeType, ISearchProvider
  {
    public IDictionary<string, string> AttributeMap { get; private set; }

    public string Name { get; private set; }

    public DeploymentComponentElement DeploymentComponent { get; private set; }

    public DeploymentDriverElement Driver { get; private set; }

    public DeploymentPackageElement DeploymentPackage { get; private set; }

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
        return "Update - " + this.Name;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public UpdateElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode1 = navigator as XmlNode;
      if (xmlNode1 == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode1.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
      this.Name = !this.AttributeMap.ContainsKey("name") ? string.Empty : this.AttributeMap["name"];
      string uri = xmlNode1.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode1.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      string xpath1 = "ns:component";
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode(xpath1, nsmgr);
      if (xmlNode2 != null)
        this.DeploymentComponent = new DeploymentComponentElement((IXPathNavigable) xmlNode2);
      string xpath2 = "ns:driver";
      XmlNode xmlNode3 = xmlNode1.SelectSingleNode(xpath2, nsmgr);
      if (xmlNode3 != null)
        this.Driver = new DeploymentDriverElement((IXPathNavigable) xmlNode3);
      string xpath3 = "ns:package";
      XmlNode xmlNode4 = xmlNode1.SelectSingleNode(xpath3, nsmgr);
      if (xmlNode4 != null)
        this.DeploymentPackage = new DeploymentPackageElement((IXPathNavigable) xmlNode4);
      this.PopulateSearchItems();
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
      this.SubSearchProviders.Add((ISearchProvider) this.DeploymentComponent);
      this.SubSearchProviders.Add((ISearchProvider) this.Driver);
      this.SubSearchProviders.Add((ISearchProvider) this.DeploymentPackage);
    }
  }
}
