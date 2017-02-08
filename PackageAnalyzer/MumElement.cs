// Decompiled with JetBrains decompiler
// Type: PackageInspector.MumElement
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
  public class MumElement : IAttributeType, ISearchProvider
  {
    public string MumFile { get; private set; }

    public IDictionary<string, string> AttributeMap { get; private set; }

    public MumAssemblyIdentityElement AssemblyIdentity { get; private set; }

    public PackageElement Package { get; private set; }

    public string DisplayName { get; private set; }

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
        return "Package Applicability - " + this.AssemblyIdentity.Name;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public MumElement(IXPathNavigable navigator, string mumFile)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      if (mumFile == null)
        throw new ArgumentNullException("mumFile");
      if (mumFile.Length == 0)
        throw new ArgumentException("mumFile cannot be of 0 length");
      XmlNode xmlNode1 = navigator as XmlNode;
      if (xmlNode1 == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.MumFile = mumFile;
      string uri = xmlNode1.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode1.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      string xpath1 = "/ns:assembly/ns:assemblyIdentity";
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode(xpath1, nsmgr);
      string xpath2 = "/ns:assembly/ns:package";
      XmlNode xmlNode3 = xmlNode1.SelectSingleNode(xpath2, nsmgr);
      this.AssemblyIdentity = new MumAssemblyIdentityElement((IXPathNavigable) xmlNode2);
      this.Package = new PackageElement((IXPathNavigable) xmlNode3);
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode1.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
      this.DisplayName = !this.AttributeMap.ContainsKey("displayName") ? string.Empty : this.AttributeMap["displayName"];
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
      this.SubSearchProviders.Add((ISearchProvider) this.Package);
      this.SubSearchProviders.Add((ISearchProvider) this.AssemblyIdentity);
    }
  }
}
