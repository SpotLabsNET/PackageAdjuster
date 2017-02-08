// Decompiled with JetBrains decompiler
// Type: PackageInspector.XsdMinInclusive
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
  public class XsdMinInclusive : ISearchProvider
  {
    public string Value { get; private set; }

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
        return "MinInclusive, " + this.Value;
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

    public XsdMinInclusive(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Value = xmlNode.Attributes["value"].Value;
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add("minInclusive");
      this.Keywords.Add(this.Value);
    }
  }
}
