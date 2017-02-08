// Decompiled with JetBrains decompiler
// Type: PackageInspector.AssemblyIdentityElement
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
  public class AssemblyIdentityElement : IAttributeType
  {
    public string Name { get; private set; }

    public string Version { get; private set; }

    public string Architecture { get; private set; }

    public string Language { get; private set; }

    public string VersionScope { get; private set; }

    public IDictionary<string, string> AttributeMap { get; private set; }

    public AssemblyIdentityElement(IXPathNavigable navigator)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      XmlNode xmlNode = navigator as XmlNode;
      if (xmlNode == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.Name = string.Empty;
      this.Version = string.Empty;
      this.Architecture = string.Empty;
      this.Language = string.Empty;
      this.VersionScope = string.Empty;
      if (xmlNode.Attributes["name"] != null)
        this.Name = xmlNode.Attributes["name"].Value;
      if (xmlNode.Attributes["version"] != null)
        this.Version = xmlNode.Attributes["version"].Value;
      if (xmlNode.Attributes["processorArchitecture"] != null)
        this.Architecture = xmlNode.Attributes["processorArchitecture"].Value;
      if (xmlNode.Attributes["language"] != null)
        this.Language = xmlNode.Attributes["language"].Value;
      if (xmlNode.Attributes["versionScope"] != null)
        this.VersionScope = xmlNode.Attributes["versionScope"].Value;
      this.AttributeMap = (IDictionary<string, string>) new Dictionary<string, string>();
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
        this.AttributeMap.Add(attribute.Name, attribute.Value);
    }
  }
}
