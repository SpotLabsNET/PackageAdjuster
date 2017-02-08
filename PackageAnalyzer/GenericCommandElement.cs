// Decompiled with JetBrains decompiler
// Type: PackageInspector.GenericCommandElement
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
  public class GenericCommandElement : IAttributeType, ISearchProvider
  {
    public IDictionary<string, string> AttributeMap { get; private set; }

    public string ExecutableName { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "Generic Command";
      }
    }

    public string SearchPath
    {
      get
      {
        return "Generic Command - " + this.ExecutableName;
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders
    {
      get
      {
        return (IList<ISearchProvider>) null;
      }
    }

    public GenericCommandElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
      this.ExecutableName = xmlNode.Attributes["executableName"].Value;
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
    }
  }
}
