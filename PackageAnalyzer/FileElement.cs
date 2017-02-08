// Decompiled with JetBrains decompiler
// Type: PackageInspector.FileElement
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
  public class FileElement : ISearchProvider
  {
    public string Name { get; private set; }

    public string SourceName { get; private set; }

    public string SourcePath { get; private set; }

    public string DestinationPath { get; private set; }

    public string ImportPath { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "File";
      }
    }

    public string SearchPath
    {
      get
      {
        return "File - " + this.Name;
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

    public FileElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Name = string.Empty;
      this.SourceName = string.Empty;
      this.SourcePath = string.Empty;
      this.DestinationPath = string.Empty;
      this.ImportPath = string.Empty;
      if (xmlNode.Attributes["name"] != null)
        this.Name = xmlNode.Attributes["name"].Value;
      if (xmlNode.Attributes["sourceName"] != null)
        this.SourceName = xmlNode.Attributes["sourceName"].Value;
      if (xmlNode.Attributes["sourcePath"] != null)
        this.SourcePath = xmlNode.Attributes["sourcePath"].Value;
      if (xmlNode.Attributes["destinationPath"] != null)
        this.DestinationPath = xmlNode.Attributes["destinationPath"].Value;
      if (xmlNode.Attributes["importPath"] != null)
        this.ImportPath = xmlNode.Attributes["importPath"].Value;
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      this.Keywords.Add(this.Name);
      this.Keywords.Add(this.SourceName);
      this.Keywords.Add(this.SourcePath);
      this.Keywords.Add(this.DestinationPath);
      this.Keywords.Add(this.ImportPath);
    }
  }
}
