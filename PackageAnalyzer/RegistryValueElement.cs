// Decompiled with JetBrains decompiler
// Type: PackageInspector.RegistryValueElement
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
  public class RegistryValueElement : ISearchProvider
  {
    public string Name { get; private set; }

    public string ValueType { get; private set; }

    public string Value { get; private set; }

    public string OperationHint { get; private set; }

    public string Owner { get; private set; }

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
        return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Registry Value - {0}, {1}, {2}", (object) this.Name, (object) this.ValueType, (object) this.Value);
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

    public RegistryValueElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Name = string.Empty;
      this.ValueType = string.Empty;
      this.Value = string.Empty;
      this.OperationHint = string.Empty;
      this.Owner = string.Empty;
      if (xmlNode.Attributes["name"] != null)
        this.Name = xmlNode.Attributes["name"].Value;
      if (xmlNode.Attributes["valueType"] != null)
        this.ValueType = xmlNode.Attributes["valueType"].Value;
      if (xmlNode.Attributes["value"] != null)
        this.Value = xmlNode.Attributes["value"].Value;
      if (xmlNode.Attributes["operationHint"] != null)
        this.OperationHint = xmlNode.Attributes["operationHint"].Value;
      if (xmlNode.Attributes["owner"] != null)
        this.Owner = xmlNode.Attributes["owner"].Value;
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add(this.Name);
      this.Keywords.Add(this.ValueType);
      this.Keywords.Add(this.Value);
      this.Keywords.Add(this.OperationHint);
      this.Keywords.Add(this.Owner);
    }
  }
}
