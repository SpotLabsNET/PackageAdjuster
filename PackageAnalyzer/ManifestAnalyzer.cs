// Decompiled with JetBrains decompiler
// Type: PackageInspector.ManifestAnalyzer
// Assembly: PackageAnalyzer, Version=1.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 08ED7AEB-6DDC-4214-9AA0-ED5C92BB11BD
// Assembly location: C:\ProgramData\SpotLabs\Utilities\mum\PackageAnalyzer.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Xml;
using System.Xml.XPath;

namespace PackageInspector
{
  public class ManifestAnalyzer : IComponentAnalyzer, IDisposable
  {
    private bool disposed;

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public IList<WindowsComponent> Analyze(string path)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (path.Length == 0)
        throw new ArgumentException("'path' parameter is of 0 length");
      if (!File.Exists(path))
        throw new FileNotFoundException(path);
      IList<WindowsComponent> components = (IList<WindowsComponent>) new List<WindowsComponent>();
      components.Clear();
      try
      {
        ManifestAnalyzer.ProcessManifest(components, path);
      }
      catch (XmlException ex)
      {
        PackageAnalyzerTracer.Instance.Write((object) ex.ToString());
        throw new PackageAnalyzerException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Error processing {0}", new object[1]
        {
          (object) path
        }), (Exception) ex);
      }
      return components;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (!this.disposed)
        return;
      int num = disposing ? 1 : 0;
      this.disposed = true;
    }

    private static void ProcessManifest(IList<WindowsComponent> components, string manifestFilePath)
    {
      FileInfo fileInfo = new FileInfo(manifestFilePath);
      using (StreamReader manReader = fileInfo.OpenText())
      {
        if (ManifestAnalyzer.ProcessComponentManifest(manReader, fileInfo.FullName, components))
          return;
        PackageAnalyzerTracer.Instance.Write((object) ("Error encountered with " + manifestFilePath + " \tContinuing..."));
      }
    }

    private static bool ProcessComponentManifest(StreamReader manReader, string manifestFilePath, IList<WindowsComponent> components)
    {
      XmlNamespaceManager namespaces;
      XmlNode xmlNode = ManifestAnalyzer.LoadPackageXml(manReader, out namespaces).SelectSingleNode("/ns:assembly/ns:assemblyIdentity", namespaces);
      if (xmlNode == null)
      {
        PackageAnalyzerTracer.Instance.Write((object) ("ERROR: Component without assemblyIdentity node detected. " + Environment.NewLine + "File: " + manifestFilePath + Environment.NewLine));
        return false;
      }
      WindowsComponent windowsComponent = new WindowsComponent((IXPathNavigable) xmlNode, manifestFilePath);
      PackageAnalyzerTracer.Instance.Write((object) windowsComponent.AssemblyIdentity.Name);
      components.Add(windowsComponent);
      return true;
    }

    private static XmlDocument LoadPackageXml(StreamReader man, out XmlNamespaceManager namespaces)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load((TextReader) man);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string uri = (string) null;
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) documentElement.Attributes)
      {
        if (attribute.Value.ToUpperInvariant().Contains("SCHEMAS-MICROSOFT-COM"))
          uri = attribute.Value;
      }
      namespaces = new XmlNamespaceManager(xmlDocument.NameTable);
      namespaces.AddNamespace("ns", uri);
      return xmlDocument;
    }
  }
}
