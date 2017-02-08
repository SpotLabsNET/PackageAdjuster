// Decompiled with JetBrains decompiler
// Type: PackageInspector.WindowsComponent
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace PackageInspector
{
  public class WindowsComponent : ISearchProvider
  {
    public string ManifestFile { get; private set; }

    public AssemblyIdentityElement AssemblyIdentity { get; private set; }

    public IList<DependencyElement> Dependencies { get; private set; }

    public IList<FileElement> Files { get; private set; }

    public IList<RegistryKeyElement> RegistryKeys { get; private set; }

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public SmiConfigurationElement SmiConfiguration { get; private set; }

    public IList<GenericCommandElement> GenericCommands { get; private set; }

    public string SearchIdentifier
    {
      get
      {
        return "Component";
      }
    }

    public string SearchPath
    {
      get
      {
        return "Component - " + string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}, {4}", (object) this.AssemblyIdentity.Name, (object) this.AssemblyIdentity.Version, (object) this.AssemblyIdentity.Architecture, (object) this.AssemblyIdentity.Language, (object) this.AssemblyIdentity.VersionScope);
      }
    }

    public IList<string> Keywords { get; private set; }

    public IList<ISearchProvider> SubSearchProviders { get; private set; }

    public WindowsComponent(IXPathNavigable navigator, string manifestFile)
    {
      if (navigator == null)
        throw new ArgumentNullException("navigator");
      if (manifestFile == null)
        throw new ArgumentNullException("manifestFile");
      if (manifestFile.Length == 0)
        throw new ArgumentException("manifestFile cannot be of 0 length");
      XmlNode xmlNode1 = navigator as XmlNode;
      if (xmlNode1 == null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "IXPathNavigable object must be of type XmlNode", new object[0]));
      this.ManifestFile = manifestFile;
      this.AssemblyIdentity = new AssemblyIdentityElement(navigator);
      this.Dependencies = (IList<DependencyElement>) new List<DependencyElement>();
      this.RegistryKeys = (IList<RegistryKeyElement>) new List<RegistryKeyElement>();
      this.Files = (IList<FileElement>) new List<FileElement>();
      this.SmiConfiguration = new SmiConfigurationElement(navigator);
      this.GenericCommands = (IList<GenericCommandElement>) new List<GenericCommandElement>();
      string uri = xmlNode1.OwnerDocument.DocumentElement.Attributes["xmlns"].Value;
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlNode1.OwnerDocument.NameTable);
      nsmgr.AddNamespace("ns", uri);
      string xpath1 = "/ns:assembly/ns:assemblyIdentity";
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode(xpath1, nsmgr);
      string xpath2 = "/ns:assembly/ns:dependency";
      foreach (IXPathNavigable selectNode in xmlNode2.SelectNodes(xpath2, nsmgr))
        this.Dependencies.Add(new DependencyElement(selectNode));
      string xpath3 = "/ns:assembly/ns:registryKeys/ns:registryKey";
      foreach (IXPathNavigable selectNode in xmlNode2.SelectNodes(xpath3, nsmgr))
        this.RegistryKeys.Add(new RegistryKeyElement(selectNode));
      string xpath4 = "/ns:assembly/ns:file";
      foreach (IXPathNavigable selectNode in xmlNode2.SelectNodes(xpath4, nsmgr))
        this.Files.Add(new FileElement(selectNode));
      string xpath5 = "/ns:assembly/ns:configuration";
      XmlNode xmlNode3 = xmlNode2.SelectSingleNode(xpath5, nsmgr);
      if (xmlNode3 != null)
        this.SmiConfiguration = new SmiConfigurationElement((IXPathNavigable) xmlNode3);
      string xpath6 = "/ns:assembly/ns:genericCommands/ns:genericCommand";
      foreach (IXPathNavigable selectNode in xmlNode2.SelectNodes(xpath6, nsmgr))
        this.GenericCommands.Add(new GenericCommandElement(selectNode));
      this.PopulateSearchItems();
    }

    private void PopulateSearchItems()
    {
      this.Keywords = (IList<string>) new List<string>();
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) this.AssemblyIdentity.AttributeMap)
      {
        this.Keywords.Add(attribute.Key);
        this.Keywords.Add(attribute.Value);
      }
      this.SubSearchProviders = (IList<ISearchProvider>) new List<ISearchProvider>();
      foreach (ISearchProvider dependency in (IEnumerable<DependencyElement>) this.Dependencies)
        this.SubSearchProviders.Add(dependency);
      foreach (ISearchProvider file in (IEnumerable<FileElement>) this.Files)
        this.SubSearchProviders.Add(file);
      foreach (ISearchProvider registryKey in (IEnumerable<RegistryKeyElement>) this.RegistryKeys)
        this.SubSearchProviders.Add(registryKey);
      foreach (ISearchProvider genericCommand in (IEnumerable<GenericCommandElement>) this.GenericCommands)
        this.SubSearchProviders.Add(genericCommand);
      this.SubSearchProviders.Add((ISearchProvider) this.SmiConfiguration);
    }
  }
}
